using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Filters.ActionFilters;
using FridgeServer.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeServer.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;

        private readonly IRepositoryManager _repositoryManager;

        private readonly IMapper _mapper;

        public ProductController(ILoggerManager loggerManager, IRepositoryManager repositoryManager,
            IMapper mapper)
        {
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        
        [HttpGet(Name = "GetProducts")/*, Authorize*/]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repositoryManager.Product
                .GetAllProductsAsync(trackChanges: false);

            if (products == null)
            {
                _loggerManager.LogError("Products in the database is null.");
                return NoContent();
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}", Name = "ProductById")]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        public Task<IActionResult> GetProduct(Guid id)
        {
            var product = HttpContext.Items["product"] as Product;

            var productDto = _mapper.Map<ProductDto>(product);
            return Task.FromResult<IActionResult>(Ok(productDto));
        }

        [HttpPost(Name = "CreateProduct")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
        {
            var productEntity = _mapper.Map<Product>(product);

            _repositoryManager.Product.CreateProduct(productEntity);
            await _repositoryManager.SaveAsync();

            var productToReturn = _mapper.Map<ProductDto>(productEntity);

            return CreatedAtRoute("ProductById", new { id = productToReturn.Id },
                productToReturn);
        }

        [HttpGet("collection/({ids})", Name = "ProductCollection")]
        public async Task<IActionResult> GetProductCollection([ModelBinder(BinderType =
            typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _loggerManager.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var productEntites = await _repositoryManager.Product.GetByIdsAsync(ids,
                trackChanges: false);

            if (ids.Count() != productEntites.Count())
            {
                _loggerManager.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var productsToReturn = _mapper.Map<IEnumerable<ProductDto>>(productEntites);
            return Ok(productsToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateProductCollection([FromBody]
            IEnumerable<ProductForCreationDto> productCollection)
        {
            if (productCollection == null)
            {
                _loggerManager.LogError("Product collection sent from client is null.");
                return BadRequest("Product collection is null.");
            }

            if (!ModelState.IsValid)
            {
                _loggerManager.LogError("Invalid model state for the productCollection" +
                    " object.");
                return UnprocessableEntity(ModelState);
            }

            var productEntities = _mapper.Map<IEnumerable<Product>>(productCollection);
            foreach (var product in productEntities)
            {
                _repositoryManager.Product.CreateProduct(product);
            }

            await _repositoryManager.SaveAsync();

            var productCollectionToReturn = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            var ids = string.Join(",", productCollectionToReturn.Select(i => i.Id));

            return CreatedAtRoute("ProductCollection", new { ids },
                productCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = HttpContext.Items["product"] as Product;

            _repositoryManager.Product.DeleteProduct(product!);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        public async Task<IActionResult> UpdateProduct(Guid id,
            [FromBody] ProductForUpdateDto product)
        {
            var productEntity = HttpContext.Items["product"] as Product;

            _mapper.Map(product, productEntity);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetFridgesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
