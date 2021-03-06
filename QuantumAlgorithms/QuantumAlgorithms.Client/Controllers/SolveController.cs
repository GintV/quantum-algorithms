﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuantumAlgorithms.Client.Api.Services;
using QuantumAlgorithms.Client.ViewModels.Solve;
using QuantumAlgorithms.Models.Get;
using static System.String;

namespace QuantumAlgorithms.Client.Controllers
{
    public class SolveController : Controller
    {
        private readonly IQuantumAlgorithmsHttpClient _quantumAlgorithmsHttpClient;
        private readonly IConfiguration _configuration;

        public SolveController(IQuantumAlgorithmsHttpClient quantumAlgorithmsHttpClient, IConfiguration configuration)
        {
            _quantumAlgorithmsHttpClient = quantumAlgorithmsHttpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> IntegerFactorization(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            try
            {
                var httpClient = await _quantumAlgorithmsHttpClient.GetClient(User.Identity.IsAuthenticated);
                var response = await httpClient.GetAsync($"Api/IntegerFactorization/{id}").ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return NotFound();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (IsNullOrEmpty(responseString))
                    throw new Exception("This should not happen.");

                var model = JsonConvert.DeserializeObject<IntegerFactorizationViewModel>(responseString);
                if (model == null)
                    throw new Exception("This should not happen.");

                model.Url = _configuration.GetSection("Applications")["API"] + "Api/IntegerFactorization/" + id;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Authentication");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DiscreteLogarithm(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            try
            {
                var httpClient = await _quantumAlgorithmsHttpClient.GetClient(User.Identity.IsAuthenticated);
                var response = await httpClient.GetAsync($"Api/DiscreteLogarithm/{id}").ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return NotFound();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (IsNullOrEmpty(responseString))
                    throw new Exception("This should not happen.");

                var model = JsonConvert.DeserializeObject<DiscreteLogarithmViewModel>(responseString);
                if (model == null)
                    throw new Exception("This should not happen.");

                model.Url = _configuration.GetSection("Applications")["API"] + "Api/DiscreteLogarithm/" + id;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Authentication");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CancelSolution(string id, string solution)
        {
            try
            {
                var httpClient = await _quantumAlgorithmsHttpClient.GetClient(User.Identity.IsAuthenticated);
                var response = await httpClient.DeleteAsync($"Api/SolutionRun/{id}").ConfigureAwait(false);
                if (solution == nameof(IntegerFactorization) || solution == nameof(DiscreteLogarithm))
                    return RedirectToAction(solution, new { Id = id });
                return RedirectToAction("Logout", "Authentication");
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Authentication");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSolution(string id, int status)
        {
            try
            {
                var httpClient = await _quantumAlgorithmsHttpClient.GetClient(User.Identity.IsAuthenticated);
                var response = await httpClient.DeleteAsync($"Api/Solution/{id}").ConfigureAwait(false);
                switch (status)
                {
                    case 0: return RedirectToAction("Enqueued", "Solutions");
                    case 1: return RedirectToAction("Processing", "Solutions");
                    case 2:
                    case 3: return RedirectToAction("Succeeded", "Solutions");
                    case 4: return RedirectToAction("Failed", "Solutions");
                    case 5: return RedirectToAction("Canceled", "Solutions");
                    default: return RedirectToAction("Logout", "Authentication");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Authentication");
            }
        }
    }
}
