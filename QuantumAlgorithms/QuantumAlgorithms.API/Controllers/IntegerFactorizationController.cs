using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using AutoMapper;
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
    public class IntegerFactorizationController : BaseController<IntegerFactorization>
    {
        private const string BasePath = ApiBasePath + PathSep + nameof(IntegerFactorization);
        private const string BasePathId = BasePath + PathSep + Id;
        private const string GetResourceRouteName = "Get" + nameof(IntegerFactorization);

        public IntegerFactorizationController(IDataService<IntegerFactorization> integerFactorizationDataService) :
            base(integerFactorizationDataService, GetResourceRouteName)
        { }

        [AllowAnonymous]
        [HttpPost(BasePath)]
        public IActionResult CreateResource([FromBody] IntegerFactorizationCreateDto createDto)
        {
            var result = base.CreateResource<IntegerFactorizationCreateDto, IntegerFactorizationGetDto>(createDto);
            if (CreatedResource == null)
                return result;

            if (User.Identity.IsAuthenticated)
                CreatedResource.SubscriberId = User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value;

            CreatedResource.JobId = CreatedResource.StartJobService();
            ResourceDataService.Update(CreatedResource);
            ResourceDataService.SaveChanges();

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
            return Ok(Map<IntegerFactorizationGetDto>(resource));
        }

        [HttpGet(BasePath)]
        public IActionResult GetResources(BaseResourceParameters baseResourceParameters, FilterByIdsParameter filterByIdsParameter,
            FilterByStatusesParameter filterByStatusesParameter) => Ok(Map<IEnumerable<IntegerFactorizationGetDto>>(ResourceDataService.
                GetManyFilter(baseResourceParameters, filterByIdsParameter, filterByStatusesParameter,
                User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value)));

        //[HttpGet(BasePath)]
        //public IActionResult GetResources(BaseResourceParameters baseResourceParameters, FilterByIdsParameter<Guid> filterByIdsParameter) =>
        //    (filterByIdsParameter?.GetIds()).Any() ? base.GetResources<IntegerFactorizationGetDto>(baseResourceParameters, filterByIdsParameter) :
        //    Ok(Array.Empty<IntegerFactorizationGetDto>());

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