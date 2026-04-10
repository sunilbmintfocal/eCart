using System.Security.Claims;
using IdentityModel;
using eCartIdentity.Data;
using eCartIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace eCartIdentity;

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
                        Email = $"{legacyUser.vchUserName}@ecartidentity.com",
                        EmailConfirmed = true,
                    };

                    var result = userMgr.CreateAsync(user, "123").Result;
                    if (result.Succeeded)
                    {
                        userMgr.AddClaimsAsync(user, new Claim[] {
                            new Claim(JwtClaimTypes.Name, legacyUser.vchName)
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
        }
    }
}
