using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace QuantumAlgorithms.API.HangfireAuthorization
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly DiscoveryClient _discoveryClient;

        public AuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
            _discoveryClient = new DiscoveryClient(_configuration.GetSection("Applications")["IDP"]);
        }

        public bool Authorize(DashboardContext context)
        {
            var doc = _discoveryClient.GetAsync().Result;

            var tokenEndpoint = doc.TokenEndpoint;
            var keys = doc.KeySet.Keys;
            var userInfoClient = new UserInfoClient(doc.UserInfoEndpoint);

            var response = userInfoClient.GetAsync(GetValidAccessToken(context.GetHttpContext()).Result).Result;
            var claims = response.Claims;

            var wtf = context.GetHttpContext();
            var user = context.GetHttpContext().User;
            var userRoles = user.Claims.FirstOrDefault(claim => claim.Type == "roles")?.Value ?? string.Empty;
            return user.Identity.IsAuthenticated && userRoles.Contains("admin");
        }

        public async Task<string> GetValidAccessToken(HttpContext context)
        {
            var expiresAtToken = await context.GetTokenAsync("expires_at");
            var expiresAt = string.IsNullOrWhiteSpace(expiresAtToken) ? DateTime.MinValue : DateTime.Parse(expiresAtToken).AddSeconds(-60).ToUniversalTime();
            string accessToken = await (expiresAt < DateTime.UtcNow ?
                RenewTokens(context) : context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken));
            return accessToken;
        }

        private async Task<string> RenewTokens(HttpContext context)
        {
            // get the current HttpContext to access the tokens

            // get the metadata
            var metaDataResponse = await _discoveryClient.GetAsync();

            // create a new token client to get new tokens
            var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint,
                "qsolverclient", "qsolverclientsecret");

            // get the saved refresh token
            var currentRefreshToken = await context
                .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            // refresh the tokens
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);

            if (!tokenResult.IsError)
            {
                // get current tokens
                var old_id_token = await context.GetTokenAsync("id_token");
                var new_access_token = tokenResult.AccessToken;
                var new_refresh_token = tokenResult.RefreshToken;

                // get new tokens and expiration time
                var tokens = new List<AuthenticationToken>();
                tokens.Add(new AuthenticationToken { Name = OpenIdConnectParameterNames.IdToken, Value = old_id_token });
                tokens.Add(new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = new_access_token });
                tokens.Add(new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = new_refresh_token });

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                tokens.Add(new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) });

                // store tokens and sign in with renewed tokens
                var info = await context.AuthenticateAsync("Cookies");
                info.Properties.StoreTokens(tokens);
                await context.SignInAsync("Cookies", info.Principal, info.Properties);

                // return the new access token 
                return tokenResult.AccessToken;
            }
            else
            {
                throw new Exception("Problem encountered while refreshing tokens.",
                    tokenResult.Exception);
            }
        }
    }
}
