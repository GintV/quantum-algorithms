﻿using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace QuantumAlgorithms.Client.Controllers
{
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View(null);
        }

        [HttpGet]
        public async Task Logout()
        {
            // get the metadata
            var discoveryClient = new DiscoveryClient(_configuration.GetSection("Applications")["IDP"]);
            var metaDataResponse = await discoveryClient.GetAsync();

            // get revocation client
            var revocationClient = new TokenRevocationClient(metaDataResponse.RevocationEndpoint, "qsolverclient", "qsolverclientsecret");

            await RevokeAccessToken(revocationClient);
            await RevokeRefreshToken(revocationClient);

            // sign-out of authentication schemes
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        private async Task RevokeAccessToken(TokenRevocationClient revocationClient)
        {
            // get access token
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                // revoke access token
                var revokeAccessTokenResponse = await revocationClient.RevokeAccessTokenAsync(accessToken);
                if (revokeAccessTokenResponse.IsError)
                {
                    throw new Exception("Error occurred during revocation of access token", revokeAccessTokenResponse.Exception);
                }
            }
        }

        private async Task RevokeRefreshToken(TokenRevocationClient revocationClient)
        {
            // get refresh token
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                // revoke refresh token
                var revokeRefreshTokenResponse = await revocationClient.RevokeRefreshTokenAsync(refreshToken);
                if (revokeRefreshTokenResponse.IsError)
                {
                    throw new Exception("Error occurred during revocation of refresh token", revokeRefreshTokenResponse.Exception);
                }
            }
        }
    }
}