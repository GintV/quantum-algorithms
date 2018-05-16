using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuantumAlgorithms.Client.Api.Services;

namespace QuantumAlgorithms.Client.Controllers
{
    [Authorize]
    public class SolutionsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SolutionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Enqueued()
        {
            return View(model: _configuration.GetSection("Applications")["API"] + "Api/Solution");
        }

        [HttpGet]
        public IActionResult Processing()
        {
            return View(model: _configuration.GetSection("Applications")["API"] + "Api/Solution");
        }

        [HttpGet]
        public IActionResult Succeeded()
        {
            return View(model: _configuration.GetSection("Applications")["API"] + "Api/Solution");
        }

        [HttpGet]
        public IActionResult Failed()
        {
            return View(model: _configuration.GetSection("Applications")["API"] + "Api/Solution");
        }

        [HttpGet]
        public IActionResult Canceled()
        {
            return View(model: _configuration.GetSection("Applications")["API"] + "Api/Solution");
        }
    }
}