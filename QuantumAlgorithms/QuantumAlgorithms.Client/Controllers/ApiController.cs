using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuantumAlgorithms.Client.Api.Services;

namespace QuantumAlgorithms.Client.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        private readonly IQuantumAlgorithmsHttpClient _quantumAlgorithmsHttpClient;

        public ApiController(IQuantumAlgorithmsHttpClient quantumAlgorithmsHttpClient)
        {
            _quantumAlgorithmsHttpClient = quantumAlgorithmsHttpClient;
        }

        public IActionResult Usage()
        {
            return View();
        }

        public async Task<IActionResult> AccessToken()
        {
            try
            {
                return Content(await _quantumAlgorithmsHttpClient.GetValidAccessToken());
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Authentication");
            }
        }
    }
}
