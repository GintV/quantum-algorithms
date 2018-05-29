using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using Hangfire;
using IdentityModel;
using QuantumAlgorithms.Models.Update;
using static QuantumAlgorithms.API.Constants;
using QuantumAlgorithms.Models.Create;
using QuantumAlgorithms.Models.Get;
using QuantumAlgorithms.API.QueryingParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.Common;
using SQLitePCL;
using static AutoMapper.Mapper;
using static QuantumAlgorithms.Models.Error.NotFoundDto;


namespace QuantumAlgorithms.API.Controllers
{
    [Authorize]
    public class SolutionController : Controller
    {
        private const string BasePath = ApiBasePath + PathSep + "Solution";
        private const string BasePathId = BasePath + PathSep + Id;
        private const string RunCancellationPath = BasePath + "Run" + PathSep + Id;
        private const string GetResourceRouteName = "Get" + "Solution";

        private readonly IDataService<DiscreteLogarithm> _discreteLogarithmDataService;
        private readonly IDataService<IntegerFactorization> _integerFactorizationDataService;
        private readonly IExecutionLogger _executionLogger;

        public SolutionController(IDataService<DiscreteLogarithm> discreteLogarithmDataService,
            IDataService<IntegerFactorization> integerFactorizationDataService, IExecutionLogger executionLogger)
        {
            _discreteLogarithmDataService = discreteLogarithmDataService;
            _integerFactorizationDataService = integerFactorizationDataService;
            _executionLogger = executionLogger;
        }

        [HttpDelete(BasePathId)]
        public IActionResult DeleteResource(Guid id)
        {
            var resource = _discreteLogarithmDataService.Get(id);
            if (resource != null)
            {
                if (resource.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                    return NotFound(ResourceNotFound(id.ToString()));

                CancelExecution(id);
                _discreteLogarithmDataService.Delete(resource);
                _discreteLogarithmDataService.SaveChanges();
            }
            else
            {
                var resource2 = _integerFactorizationDataService.Get(id);
                if (resource2 == null || resource2.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                    return NotFound(ResourceNotFound(id.ToString()));

                CancelExecution(id);
                _integerFactorizationDataService.Delete(resource2);
                _integerFactorizationDataService.SaveChanges();
            }
            return NoContent();
        }

        [HttpGet(BasePathId, Name = GetResourceRouteName)]
        public IActionResult GetResource(Guid id)
        {
            var resource = _discreteLogarithmDataService.Get(id);
            if (resource != null)
            {
                if (resource.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                    return NotFound(ResourceNotFound(id.ToString()));
                return Ok(Map<DiscreteLogarithmGetDto>(resource));
            }

            var resource2 = _integerFactorizationDataService.Get(id);
            if (resource2 == null || resource2.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                return NotFound(ResourceNotFound(id.ToString()));
            return Ok(Map<IntegerFactorizationGetDto>(resource2));
        }

        [HttpGet(BasePath)]
        public IActionResult GetResources(BaseResourceParameters baseResourceParameters, FilterByIdsParameter filterByIdsParameter,
            FilterByStatusesParameter filterByStatusesParameter)
        {
            var subscriberId = User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value;

            IEnumerable<QuantumAlgorithmGetDto> discreteLogarithms =
                Map<IEnumerable<DiscreteLogarithmGetDto>>(_discreteLogarithmDataService.GetManyFilter(baseResourceParameters,
                filterByIdsParameter, filterByStatusesParameter, subscriberId, false));

            IEnumerable<QuantumAlgorithmGetDto> integerFactorizations =
                Map<IEnumerable<IntegerFactorizationGetDto>>(_integerFactorizationDataService.GetManyFilter(baseResourceParameters,
                filterByIdsParameter, filterByStatusesParameter, subscriberId, false));

            List<QuantumAlgorithmGetDto> solutions = discreteLogarithms.ToList();
            solutions.AddRange(integerFactorizations);
            return Ok(solutions.OrderByDescending(solution => solution.StartTime).AsQueryable().ApplyPaging(baseResourceParameters));
        }

        [HttpDelete(RunCancellationPath)]
        public IActionResult CancelExecution(Guid id)
        {
            var resource = _discreteLogarithmDataService.Get(id);
            if (resource != null)
            {
                if (resource.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                    return NotFound(ResourceNotFound(id.ToString()));

                //if (resource.InnerJobId != null)
                //    BackgroundJob.Delete(resource.InnerJobId);

                //BackgroundJob.Delete(resource.JobId);

                _executionLogger.SetExecutionId(id);
                _executionLogger.Error("Execution canceled by the user.");

                resource.Status = Status.Canceled;
                resource.FinishTime = DateTime.Now;
                resource.CancelJob = true;
                _discreteLogarithmDataService.Update(resource);
                _discreteLogarithmDataService.SaveChanges();
            }
            else
            {
                var resource2 = _integerFactorizationDataService.Get(id);
                if (resource2 == null || resource2.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                    return NotFound(ResourceNotFound(id.ToString()));

                //if (resource2.InnerJobId != null)
                //    BackgroundJob.Delete(resource2.InnerJobId);

                //BackgroundJob.Delete(resource2.JobId);

                _executionLogger.SetExecutionId(id);
                _executionLogger.Error("Execution canceled by the user.");

                resource2.Status = Status.Canceled;
                resource2.FinishTime = DateTime.Now;
                resource2.CancelJob = true;
                _integerFactorizationDataService.Update(resource2);
                _integerFactorizationDataService.SaveChanges();
            }
            return NoContent();
        }
    }
}