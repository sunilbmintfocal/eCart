using System.Security.Claims;
using IdentityModel;
using MintCart.Data;
using MintCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Serilog;

namespace MintCart;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            // Migrate users from Usermaster
            var legacyUsers = context.Usermasters.Where(u => u.bitIsActive).ToList();
            foreach (var legacyUser in legacyUsers)
            {
                var existingUser = userMgr.FindByNameAsync(legacyUser.vchUserName).Result;
                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = legacyUser.vchUserName,
                        Email = $"{legacyUser.vchUserName}@MintCart.com",
                        EmailConfirmed = true,
                    };

                    var result = userMgr.CreateAsync(user, "123").Result;
                    if (result.Succeeded)
                    {
                        userMgr.AddClaimsAsync(user, new Claim[] {
                            new Claim(JwtClaimTypes.Name, legacyUser.vchName),
                            new Claim(JwtClaimTypes.Email, user.Email)
                        }).Wait();
                        Log.Debug("Migrated legacy user: {username}", legacyUser.vchUserName);
                    }
                    else
                    {
                        Log.Error("Failed to migrate user {username}: {error}", legacyUser.vchUserName, result.Errors.First().Description);
                    }
                }
            }
            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
            var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            configContext.Database.Migrate();

            // Sync Clients
            foreach (var client in Config.GetClients(app.Configuration))
            {
                var existingClient = configContext.Clients
                    .Include(x => x.AllowedScopes)
                    .Include(x => x.RedirectUris)
                    .Include(x => x.PostLogoutRedirectUris)
                    .Include(x => x.AllowedCorsOrigins)
                    .FirstOrDefault(x => x.ClientId == client.ClientId);

                if (existingClient == null)
                {
                    configContext.Clients.Add(client.ToEntity());
                    Log.Debug("Seeded new client: {ClientId}", client.ClientId);
                }
                else
                {
                    // Update existing client properties
                    existingClient.ClientName = client.ClientName;
                    
                    // Sync RedirectUris
                    existingClient.RedirectUris.Clear();
                    foreach (var uri in client.RedirectUris)
                    {
                        existingClient.RedirectUris.Add(new Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri { RedirectUri = uri });
                    }

                    // Sync PostLogoutRedirectUris
                    existingClient.PostLogoutRedirectUris.Clear();
                    foreach (var uri in client.PostLogoutRedirectUris)
                    {
                        existingClient.PostLogoutRedirectUris.Add(new Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri { PostLogoutRedirectUri = uri });
                    }

                    // Sync AllowedCorsOrigins
                    existingClient.AllowedCorsOrigins.Clear();
                    foreach (var origin in client.AllowedCorsOrigins)
                    {
                        existingClient.AllowedCorsOrigins.Add(new Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin { Origin = origin });
                    }

                    // Sync AllowedScopes
                    existingClient.AllowedScopes.Clear();
                    foreach (var scopeName in client.AllowedScopes)
                    {
                        existingClient.AllowedScopes.Add(new Duende.IdentityServer.EntityFramework.Entities.ClientScope { Scope = scopeName });
                    }

                    Log.Debug("Updated existing client configuration for: {ClientId}", client.ClientId);
                }
            }
            configContext.SaveChanges();

            // Sync IdentityResources
            foreach (var resource in Config.IdentityResources)
            {
                if (!configContext.IdentityResources.Any(x => x.Name == resource.Name))
                {
                    configContext.IdentityResources.Add(resource.ToEntity());
                    Log.Debug("Seeded new identity resource: {Name}", resource.Name);
                }
            }
            configContext.SaveChanges();

            // Sync ApiScopes
            foreach (var scopeResource in Config.ApiScopes)
            {
                if (!configContext.ApiScopes.Any(x => x.Name == scopeResource.Name))
                {
                    configContext.ApiScopes.Add(scopeResource.ToEntity());
                    Log.Debug("Seeded new api scope: {Name}", scopeResource.Name);
                }
            }
            configContext.SaveChanges();

            // Sync ApiResources
            foreach (var resource in Config.ApiResources)
            {
                var existingResource = configContext.ApiResources
                    .Include(x => x.UserClaims)
                    .Include(x => x.Scopes)
                    .FirstOrDefault(x => x.Name == resource.Name);

                if (existingResource == null)
                {
                    configContext.ApiResources.Add(resource.ToEntity());
                    Log.Debug("Seeded new api resource: {Name}", resource.Name);
                }
                else
                {
                    // Sync User Claims
                    existingResource.UserClaims.Clear();
                    foreach (var claim in resource.UserClaims)
                    {
                        existingResource.UserClaims.Add(new Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim { Type = claim });
                    }
                    Log.Debug("Updated claims for api resource: {Name}", resource.Name);
                }
            }
            configContext.SaveChanges();

            var persistedGrantContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            persistedGrantContext.Database.Migrate();
        }
    }
}
