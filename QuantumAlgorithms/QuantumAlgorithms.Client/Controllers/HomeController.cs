using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuantumAlgorithms.Client.Api.Services;
using QuantumAlgorithms.Client.ViewModels;
using QuantumAlgorithms.Client.ViewModels.Home;
using QuantumAlgorithms.Client.ViewModels.Shared;
using QuantumAlgorithms.Models.Create;
using QuantumAlgorithms.Models.Get;

namespace QuantumAlgorithms.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuantumAlgorithmsHttpClient _quantumAlgorithmsHttpClient;
        private readonly IConfiguration _configuration;

        public HomeController(IQuantumAlgorithmsHttpClient quantumAlgorithmsHttpClient, IConfiguration configuration)
        {
            _quantumAlgorithmsHttpClient = quantumAlgorithmsHttpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Solve()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Solve(SolveViewModel model)
        {
            if (model.Problem == Problem.IntegerFactorization)
            {
                if (ModelState[nameof(SolveViewModel.Number)].Errors.Any())
                {
                    model.IsValid = false;
                    ModelState.Clear();
                    return View(model);
                }

                try
                {
                    var httpClient = await _quantumAlgorithmsHttpClient.GetClient(User.Identity.IsAuthenticated);
                    var request = new IntegerFactorizationCreateDto {Number = model.Number};
                    var response = await httpClient.PostAsync("Api/IntegerFactorization", new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8, "application/json"));

                    if (!response.IsSuccessStatusCode)
                    {
                        model.ApiRequestFailed = true;
                        ModelState.Clear();
                        return View(model);
                    }

                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var executionId = JsonConvert.DeserializeObject<IntegerFactorizationGetDto>(responseString).Id;

                    return RedirectToAction("IntegerFactorization", "Solve", new {Id = executionId});
                }
                catch (Exception)
                {
                    return RedirectToAction("Logout", "Authentication");
                }
            }

            if (model.Problem == Problem.DiscreteLogarithm)
            {
                if (ModelState[nameof(SolveViewModel.Result)].Errors.Any() ||
                    ModelState[nameof(SolveViewModel.Generator)].Errors.Any() ||
                    ModelState[nameof(SolveViewModel.Modulus)].Errors.Any())
                {
                    model.IsValid = false;
                    ModelState.Clear();
                    return View(model);
                }

                try
                {
                    var httpClient = await _quantumAlgorithmsHttpClient.GetClient(User.Identity.IsAuthenticated);
                    var request = new DiscreteLogarithmCreateDto
                    {
                        Result = model.Result,
                        Generator = model.Generator,
                        Modulus = model.Modulus
                    };
                    var response = await httpClient.PostAsync("Api/DiscreteLogarithm", new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8, "application/json"));

                    if (!response.IsSuccessStatusCode)
                    {
                        model.ApiRequestFailed = true;
                        ModelState.Clear();
                        return View(model);
                    }

                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var executionId = JsonConvert.DeserializeObject<IntegerFactorizationGetDto>(responseString).Id;

                    return RedirectToAction("DiscreteLogarithm", "Solve", new {Id = executionId});
                }
                catch (Exception)
                {
                    return RedirectToAction("Logout", "Authentication");
                }
            }

            return View();
        }
    }
}
