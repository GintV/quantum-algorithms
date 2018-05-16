using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;

namespace Ginsoft.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "1, Main Road"),
                        new Claim("role", "FreeUser"),
                        new Claim("course", "English"),
                        new Claim("subscriptionlevel", "FreeUser"),
                        new Claim("country", "be")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Claire"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "2, Big Street"),
                        new Claim("role", "PayingUser"),
                        new Claim("course", "French"),
                        new Claim("subscriptionlevel", "PayingUser"),
                        new Claim("country", "nl")
                    }
                }
            };
        }

        internal static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("qsolverapi", "QSolver API", new [] { "role" })
                {
                    ApiSecrets = new [] {new Secret("qsolverapisecret".Sha256()) }
                }
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
               new IdentityResources.Address(),
               new IdentityResource("courses", "Your course(s)", new []{"course"}),
               new IdentityResource("roles", "Your role(s)", new []{"role"}),
               new IdentityResource("subscriptionlevel", "Your subscription level", new[]{"subscriptionlevel"}),
               new IdentityResource("country", "Your country", new[]{"country" }),
            };
        }

        public static List<Client> GetClients(IConfigurationSection applicationsConfigurationSection)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "QSolver",
                    ClientId = "qsolverclient",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenType = AccessTokenType.Reference,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    RedirectUris = new List<string>
                    {
                        applicationsConfigurationSection["Client"] + "signin-oidc"
                    },
                    PostLogoutRedirectUris = new[]{ applicationsConfigurationSection["Client"] + "signout-callback-oidc" },
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        //IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        //"courses",
                        "qsolverapi",
                        //"subscriptionlevel",
                    },
                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 3600,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("qsolverclientsecret".Sha256())
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client
                {
                    ClientName = "QSolver API",
                    ClientId = "qsolverapi",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenType = AccessTokenType.Reference,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    RedirectUris = new List<string>
                    {
                        applicationsConfigurationSection["API"] + "signin-oidc"
                    },
                    PostLogoutRedirectUris = new[]{ applicationsConfigurationSection["API"] + "signout-callback-oidc" },
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        //"courses",
                        //"subscriptionlevel",
                    },
                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 3600,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("qsolverapisecret".Sha256())
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
