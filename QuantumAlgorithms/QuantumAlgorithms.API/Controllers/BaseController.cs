using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.Models.Create;
using QuantumAlgorithms.Models.Get;
using QuantumAlgorithms.Models.Update;
using QuantumAlgorithms.API.QueryingParameters;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.Domain;
using static AutoMapper.Mapper;
using QuantumAlgorithms.DataService;
using static QuantumAlgorithms.Models.Error.BadRequestDto;
using static QuantumAlgorithms.Models.Error.NotFoundDto;

namespace QuantumAlgorithms.API.Controllers
{
    public abstract class BaseController<TEntity> : Controller
        where TEntity : class, IEntity
    {
        private readonly string _getResourceRouteName;

        protected TEntity CreatedResource { get; private set; }
        protected IDataService<TEntity> ResourceDataService { get; }
        protected IExecutionLogger ExecutionLogger { get; }

        protected BaseController(IDataService<TEntity> resourceDataService, IExecutionLogger executionLogger, string getResourceRouteName)
        {
            ResourceDataService = resourceDataService;
            ExecutionLogger = executionLogger;
            _getResourceRouteName = getResourceRouteName;
        }

        protected IActionResult CreateResource<TCreateDto, TGetDto>(TCreateDto createDto)
            where TCreateDto : ICreateDto<TEntity>
            where TGetDto : IGetDto<TEntity>
        {
            if (createDto == null)
                return BadRequest(InvalidData());
            if (!ModelState.IsValid)
                return BadRequest(InvalidData()); // TODO: -- 400 missing required fields, consider mentioning which
            var resource = Map<TEntity>(createDto);
            var result = ResourceDataService.AreRelationshipsValid(resource);
            if (!result.IsValid)
                return NotFound(ParentNotFound(result.NotFoundParentId));
            ResourceDataService.Create(resource);
            ResourceDataService.SaveChanges();
            CreatedResource = resource;
            var resourceToReturn = Map<TGetDto>(resource);
            return CreatedAtRoute(_getResourceRouteName, new { id = resourceToReturn.Id }, resourceToReturn);
        }

        protected IActionResult CreateResource(Guid id) => ResourceDataService.Get(id) == null ?
            NotFound() : new StatusCodeResult(StatusCodes.Status409Conflict);

        protected IActionResult DeleteResource(Guid id)
        {
            var resource = ResourceDataService.Get(id);
            if (resource == null)
                return NotFound(ResourceNotFound(id.ToString()));
            ResourceDataService.Delete(resource);
            ResourceDataService.SaveChanges();
            return NoContent();
        }

        protected IActionResult GetResource<TGetDto>(Guid id)
            where TGetDto : IGetDto<TEntity>
        {
            var resource = ResourceDataService.Get(id);
            if (resource == null)
                return NotFound(ResourceNotFound(id.ToString()));
            return Ok(Map<TGetDto>(resource));
        }

        //protected IActionResult GetResources<TGetDto>(BaseResourceParameters baseResourceParameters,
        //    FilterByIdsParameter<TIdentifier> filterByIdsParameters) where TGetDto : IGetDto<TEntity, TIdentifier>
        //{
        //    return Ok(Map<IEnumerable<TGetDto>>(ResourceDataService.GetManyFilter(baseResourceParameters, filterByIdsParameters)));
        //}

        protected IActionResult UpdateResource<TUpdateDto, TGetDto>(Guid id, TUpdateDto updateDto)
            where TUpdateDto : IUpdateDto<TEntity>
            where TGetDto : IGetDto<TEntity>
        {
            var resource = ResourceDataService.Get(id);
            if (resource == null)
                return NotFound(ResourceNotFound(id.ToString()));
            if (!ModelState.IsValid)
                return BadRequest(InvalidData());
            Map(updateDto, resource);
            var result = ResourceDataService.AreRelationshipsValid(resource);
            if (!result.IsValid)
                return NotFound(ParentNotFound(result.NotFoundParentId));
            ResourceDataService.Update(resource);
            ResourceDataService.SaveChanges();
            return Ok(Map<TGetDto>(resource));
        }
    }
}
