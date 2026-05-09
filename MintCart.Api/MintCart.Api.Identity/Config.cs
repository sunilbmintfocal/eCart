using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace MintCart;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("MintCart-api", "MintCart Web API")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("MintCart-api", "MintCart Web API")
            {
                Scopes = { "MintCart-api" },
                UserClaims = { "name", "email" }
            }
        };

    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        var webUIUrls = configuration.GetSection("ClientUrls:WebUI").Get<string[]>() ?? new[] { "https://localhost:5173", "http://localhost:5173" };
        
        return new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },

            // WebUI client (SPA)
            new Client
            {
                ClientId = "MintCart-webui",
                ClientName = "MintCart Web Management",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris = webUIUrls.SelectMany(url => new[] { $"{url}/callback", $"{url}/silent-renew", url }).ToList(),
                PostLogoutRedirectUris = webUIUrls.ToList(),
                AllowedCorsOrigins = webUIUrls.ToList(),
                AlwaysIncludeUserClaimsInIdToken = true,

                AllowedScopes = { "openid", "profile", "email", "MintCart-api" },
                AllowOfflineAccess = true
            },
        };
    }
}
