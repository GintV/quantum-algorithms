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
using QuantumAlgorithms.API.Models.Update;
using static QuantumAlgorithms.API.Constants;
using QuantumAlgorithms.API.Models.Create;
using QuantumAlgorithms.API.Models.Get;
using QuantumAlgorithms.API.QueryingParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.Common;


namespace QuantumAlgorithms.API.Controllers
{
    public class IntegerFactorizationController : BaseController<IntegerFactorization, Guid>
    {
        private const string BasePath = ApiBasePath + PathSep + nameof(IntegerFactorization);
        private const string BasePathId = BasePath + PathSep + Id;
        private const string GetResourceRouteName = "Get" + nameof(IntegerFactorization);

        public IntegerFactorizationController(IDataService<IntegerFactorization, Guid> integerFactorizationDataService) :
            base(integerFactorizationDataService, GetResourceRouteName)
        { }

        [HttpPost(BasePath)]
        public IActionResult CreateResource([FromBody] IntegerFactorizationCreateDto createDto)
        {
            var result = base.CreateResource<IntegerFactorizationCreateDto, IntegerFactorizationGetDto>(createDto);
            CreatedResource?.StartJobService();
            return result;
        }

        [HttpPost(BasePathId)]
        public new IActionResult CreateResource(Guid id) => base.CreateResource(id);

        [HttpDelete(BasePathId)]
        public new IActionResult DeleteResource(Guid id) => base.DeleteResource(id);

        [HttpGet(BasePathId, Name = GetResourceRouteName)]
        public IActionResult GetResource(Guid id) => base.GetResource<IntegerFactorizationGetDto>(id);

        //[HttpGet(BasePath), EnableCors("AllowMyClient")]
        //public IActionResult GetResources(BaseResourceParameters baseResourceParameters, FilterByIdsParameters<Guid> filterByIdsParameters) =>
        //    base.GetResources<IntegerFactorizationGetDto>(baseResourceParameters, filterByIdsParameters);

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