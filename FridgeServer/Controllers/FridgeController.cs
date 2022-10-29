using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Filters.ActionFilters;
using FridgeServer.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeServer.Controllers
{
    [ApiController]

    [Route("api/fridges")]
    
    [ApiExplorerSettings(GroupName = "v1")]
    public class FridgeController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;

        private readonly IRepositoryManager _repositoryManager;

        private readonly IMapper _mapper;

        public FridgeController(ILoggerManager loggerManager, IRepositoryManager repositoryManager,
            IMapper mapper)
        {
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of all fridges.
        /// </summary>
        /// <returns>The fridges list.</returns>
        
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFridges()
        {
            var fridges = await _repositoryManager.Fridge.GetAllFridgesAsync(trackChanges: false);

            if (fridges == null)
            {
                _loggerManager.LogError("Fridges in the database is null.");
                return NoContent();
            }

            var fridgesDto = _mapper.Map<IEnumerable<FridgeDto>>(fridges);

            return Ok(fridgesDto);
        }


        [HttpGet("{id}", Name = "FridgeById")]
        [ServiceFilter(typeof(ValidateFridgeExistsAttribute))]
        public Task<IActionResult> GetFridge(Guid id)
        {
            var fridge = HttpContext.Items["fridge"] as Fridge;

            var fridgeDto = _mapper.Map<FridgeDto>(fridge);
            return Task.FromResult<IActionResult>(Ok(fridgeDto));
        }

        [HttpPost(Name = "CreateFridge")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateFridge([FromBody] FridgeForCreationDto fridge)
        {
            var fridgeEntity = _mapper.Map<Fridge>(fridge);

            _repositoryManager.Fridge.CreateFridge(fridgeEntity);
            await _repositoryManager.SaveAsync();

            var fridgeToReturn = _mapper.Map<FridgeDto>(fridgeEntity);

            return CreatedAtRoute("FridgeById", new { id = fridgeToReturn.Id }, fridgeToReturn);
        }

        [HttpGet("collection/({ids})", Name = "FridgeCollection")]
        public async Task<IActionResult> GetFridgeCollection([ModelBinder(BinderType =
            typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _loggerManager.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var fridgeEntites = await _repositoryManager.Fridge.GetByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != fridgeEntites.Count())
            {
                _loggerManager.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var fridgesToReturn = _mapper.Map<IEnumerable<FridgeDto>>(fridgeEntites);
            return Ok(fridgesToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateFridgeCollection([FromBody]
            IEnumerable<FridgeForCreationDto> fridgeCollection)
        {
            if (fridgeCollection == null)
            {
                _loggerManager.LogError("Fridge collection sent from client is null.");
                return BadRequest("Fridge collection is null.");
            }

            if (!ModelState.IsValid)
            {
                _loggerManager.LogError("Invalid model state for the fridgeCollection" +
                    " object.");
                return UnprocessableEntity(ModelState);
            }

            var fridgeEntities = _mapper.Map<IEnumerable<Fridge>>(fridgeCollection);
            foreach (var fridge in fridgeEntities)
            {
                _repositoryManager.Fridge.CreateFridge(fridge);
            }

            await _repositoryManager.SaveAsync();

            var fridgeCollectionToReturn = _mapper.Map<IEnumerable<FridgeDto>>(fridgeEntities);
            var ids = string.Join(",", fridgeCollectionToReturn.Select(i => i.Id));

            return CreatedAtRoute("FridgeCollection", new { ids }, fridgeCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateFridgeExistsAttribute))]
        public async Task<IActionResult> DeleteFridge(Guid id)
        {
            var fridge = HttpContext.Items["fridge"] as Fridge;

            _repositoryManager.Fridge.DeleteFridge(fridge!);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateFridgeExistsAttribute))]
        public async Task<IActionResult> UpdateFridge(Guid id,
            [FromBody] FridgeForUpdateDto fridge)
        {
            var fridgeEntity = HttpContext.Items["fridge"] as Fridge;

            _mapper.Map(fridge, fridgeEntity);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public async Task<IActionResult> GetFridgesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");

            return Ok();
        }
    }
}
