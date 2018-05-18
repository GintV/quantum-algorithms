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
using static AutoMapper.Mapper;
using static QuantumAlgorithms.Models.Error.NotFoundDto;


namespace QuantumAlgorithms.API.Controllers
{
    [Authorize]
    public class DiscreteLogarithmController : BaseController<DiscreteLogarithm>
    {
        private const string BasePath = ApiBasePath + PathSep + nameof(DiscreteLogarithm);
        private const string BasePathId = BasePath + PathSep + Id;
        private const string RunCancellationPath = BasePath + "Run" + PathSep + Id;
        private const string GetResourceRouteName = "Get" + nameof(DiscreteLogarithm);

        public DiscreteLogarithmController(IDataService<DiscreteLogarithm> discreteLogarithmDataService, IExecutionLogger executionLogger) :
            base(discreteLogarithmDataService, executionLogger, GetResourceRouteName)
        { }

        [AllowAnonymous]
        [HttpPost(BasePath)]
        public IActionResult CreateResource([FromBody] DiscreteLogarithmCreateDto createDto)
        {
            var result = base.CreateResource<DiscreteLogarithmCreateDto, DiscreteLogarithmGetDto>(createDto);
            if (CreatedResource == null)
                return result;

            if (User.Identity.IsAuthenticated)
                CreatedResource.SubscriberId = User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value;

            CreatedResource.JobId = CreatedResource.StartJobService();
            ResourceDataService.Update(CreatedResource);
            ResourceDataService.SaveChanges();

            ExecutionLogger.SetExecutionId(CreatedResource.Id);
            ExecutionLogger.Info("Enqueued for execution.");

            return result;
        }

        [HttpPost(BasePathId)]
        public new IActionResult CreateResource(Guid id) => base.CreateResource(id);

        [HttpDelete(BasePathId)]
        public new IActionResult DeleteResource(Guid id)
        {
            var resource = ResourceDataService.Get(id);
            if (resource == null || resource.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                return NotFound(ResourceNotFound(id.ToString()));

            CancelExecution(id);
            ResourceDataService.Delete(resource);
            ResourceDataService.SaveChanges();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet(BasePathId, Name = GetResourceRouteName)]
        public IActionResult GetResource(Guid id)
        {
            var resource = ResourceDataService.Get(id);
            if (resource == null || resource.SubscriberId != null && resource.SubscriberId != User?.Claims?.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Subject)?.Value)
                return NotFound(ResourceNotFound(id.ToString()));
            return Ok(Map<DiscreteLogarithmGetDto>(resource));
        }

        [HttpGet(BasePath)]
        public IActionResult GetResources(BaseResourceParameters baseResourceParameters, FilterByIdsParameter filterByIdsParameter,
            FilterByStatusesParameter filterByStatusesParameter) =>
            Ok(Map<IEnumerable<DiscreteLogarithmGetDto>>(ResourceDataService.GetManyFilter(baseResourceParameters, filterByIdsParameter,
                filterByStatusesParameter, User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)));

        [HttpDelete(RunCancellationPath)]
        public IActionResult CancelExecution(Guid id)
        {
            var resource = ResourceDataService.Get(id);
            if (resource == null || resource.SubscriberId != User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)
                return NotFound(ResourceNotFound(id.ToString()));
            if (resource.Status == Status.Enqueued || resource.Status == Status.InProgress)
            {
                //if(resource.InnerJobId != null)
                //    BackgroundJob.Delete(resource.InnerJobId);

                //BackgroundJob.Delete(resource.JobId);

                ExecutionLogger.SetExecutionId(id);
                ExecutionLogger.Error("Execution canceled by the user.");

                resource.Status = Status.Canceled;
                resource.CancelJob = true;
                ResourceDataService.Update(resource);
                ResourceDataService.SaveChanges();
            }
            return NoContent();
        }

        //[HttpGet(BasePath)]
        //public IActionResult GetResources(BaseResourceParameters baseResourceParameters, FilterByIdsParameter<Guid> filterByIdsParameter) =>
        //    (filterByIdsParameter?.GetIds()).Any() ? base.GetResources<DiscreteLogarithmGetDto>(baseResourceParameters, filterByIdsParameter) :
        //    Ok(Array.Empty<DiscreteLogarithmGetDto>());

        //[HttpPut(BasePathId)]
        //public IActionResult UpdateResource(Guid id, [FromBody] PlayerUpdateDto updateDto) =>
        //    base.UpdateResource<PlayerUpdateDto, PlayerGetDto>(id, updateDto);

        //private IEnumerable<(string column, string value)> Filter(string userId)
        //{
        //    if (userId != null)
        //        yield return (nameof(userId), userId);
        //}
    }
}