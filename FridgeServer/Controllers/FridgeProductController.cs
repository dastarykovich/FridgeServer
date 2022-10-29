using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeServer.Controllers
{
    [Route("api/fridges/{fridgeId}/products")]
    [ApiController]
    public class FridgeProductController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;

        private readonly IRepositoryManager _repositoryManager;

        private readonly IMapper _mapper;

        public FridgeProductController(ILoggerManager loggerManager,
            IRepositoryManager repositoryManager, IMapper mapper)
        {
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsForFridge( Guid fridgeId)
        {
            var fridge = await _repositoryManager.Fridge.GetFridgeAsync(fridgeId,
                trackChanges: false);

            if (fridge == null)
            {
                _loggerManager.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the" +
                    $" database.");
                return NotFound();
            }

            var fridgeProductsFromDb = await _repositoryManager.FridgeProduct
                .GetFridgeProductsAsync(trackChanges: false);

            if (fridgeProductsFromDb == null)
            {
                _loggerManager.LogInfo($"FridgeProducts for Fridge with id: {fridgeId} doesn't" +
                    $" exist in the database.");
                return NotFound();
            }

            var products = await _repositoryManager.Product
                .GetByIdsAsync(fridgeProductsFromDb.Select(fp => fp.ProductId),
                    false);

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }


        [HttpGet("{id}", Name = "GetProductForFridge")]
        [ServiceFilter(typeof(ValidateProductForFridgeExistsAttribute))]
        public Task<IActionResult> GetProductForFridge(Guid fridgeId, Guid id)
        {
            var productDb = HttpContext.Items["product"] as Product;

            var product = _mapper.Map<ProductDto>(productDb);
            return Task.FromResult<IActionResult>(Ok(product));
        }

        [HttpPost("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateProductForFridgeExistsAttribute))]
        public async Task<IActionResult> CreateProductForFridge(Guid fridgeId, Guid id,
            [FromBody] FridgeProductForCreationDto fridgeProductForCreationDto)
        {
            var product = HttpContext.Items["product"] as Product;

            var fridgeProductEntity = _mapper.Map<FridgeProduct>(fridgeProductForCreationDto);
            _repositoryManager.FridgeProduct.CreateFridgeProduct(fridgeId, id, fridgeProductEntity);
            await _repositoryManager.SaveAsync();

            var productToReturn = _mapper.Map<ProductDto>(product);
            return CreatedAtRoute("GetProductForFridge", new { fridgeId,
                id = productToReturn.Id }, productToReturn);
        }

        [HttpDelete]
        [Route("api/fridgeProducts/{fridgeProductId}")]
        public async Task<IActionResult> DeleteProductForFridge(Guid fridgeProductId)
        {
            var fridgeProductFromDb = await _repositoryManager.FridgeProduct
                .GetFridgeProductAsync(fridgeProductId, trackChanges: false);

            if (fridgeProductFromDb == null)
            {
                _loggerManager.LogInfo($"FridgeProduct with id: {fridgeProductId} doesn't" +
                                       $" exist in the database.");
                return NotFound();
            }

            _repositoryManager.FridgeProduct.DeleteFridgeProduct(fridgeProductFromDb!);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("api/fridgeProducts/{fridgeProductId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProductForFridge(Guid fridgeProductId,
            [FromBody] FridgeProductForUpdateDto fridgeProductForUpdateDto)
        {
            var fridgeProductFromDb = await _repositoryManager.FridgeProduct
                .GetFridgeProductAsync(fridgeProductId, trackChanges: false);

            if (fridgeProductFromDb == null)
            {
                _loggerManager.LogInfo($"FridgeProduct with id: {fridgeProductId} doesn't" +
                                       $" exist in the database.");
                return NotFound();
            }

            _mapper.Map(fridgeProductForUpdateDto, fridgeProductFromDb);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}
