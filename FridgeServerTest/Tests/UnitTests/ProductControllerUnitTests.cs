using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Filters.ActionFilters;
using FridgeServerTest.MoqObjects;
using FridgeServer.AutoMapperProfile;
using FridgeServer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace FridgeServerTest.Tests.UnitTests;

public class ProductControllerUnitTests
{
        private readonly ProductController _controller;
        private readonly RepositoryManagerMock _repository;
        private readonly Mock<ActionExecutionDelegate> _actionExecutionDelegate;
        private readonly ActionContext _actionContext;
        private readonly ValidationFilterAttribute _validationFilterAttribute;
        private readonly ValidateProductExistsAttribute _validateProductExistsAttribute;

        public ProductControllerUnitTests()
        {
            _repository = new RepositoryManagerMock();
            var loggerManagerMock = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var httpContext = new DefaultHttpContext();
            _controller = new ProductController(loggerManagerMock.Object, _repository, mapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            _actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            _actionContext = new ActionContext(
                _controller.HttpContext,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                Mock.Of<ModelStateDictionary>()
            );

            _validationFilterAttribute = new ValidationFilterAttribute(
                loggerManagerMock.Object);
            _validateProductExistsAttribute = new ValidateProductExistsAttribute(
                _repository, loggerManagerMock.Object);
        }

        [Fact]
        public async void GetProducts_WhenCalled_ReturnsAllProducts()
        {
            //Act
            var okResult = await _controller.GetProducts() as OkObjectResult;

            //Assert
            var items = Assert.IsType<List<ProductDto>>(okResult?.Value);
            Assert.Equal(5, items.Count());
        }

        [Fact]
        public async void GetProduct_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", Guid.NewGuid() }
                },
                _controller
            );

            // Act
            await _validateProductExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetProduct_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var testGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", testGuid }
                },
                _controller
            );

            // Act
            await _validateProductExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var okResult = await _controller.GetProduct(testGuid);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetProduct_ExistingGuidPassed_ReturnRightItem()
        {
            //Arrange
            var testGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", testGuid }
                },
                _controller
            );

            // Act
            await _validateProductExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var okResult = await _controller.GetProduct(testGuid) as OkObjectResult;

            //Assert
            Assert.IsType<ProductDto>(okResult?.Value);
            Assert.Equal(testGuid, ((okResult?.Value as ProductDto)!).Id);
        }

        [Fact]
        public void CreateProduct_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var defaultQuantityMissingItem = new ProductForCreationDto
            {
                Name = "Apple",
                DefaultQuantity = 1
            };

            _actionContext.ModelState.AddModelError("DefaultQuantity", "Required");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"ProductForCreationDto", defaultQuantityMissingItem }
                },
                _controller
            );

            //Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var unprocessableEntityResponse = actionExecutingContext.Result;

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(unprocessableEntityResponse);
        }

        [Fact]
        public async void CreateProduct_ValidObjectPassed_ReturnsCreatedResponse()
        {
            //Arrange
            var testItem = new ProductForCreationDto
            {
                Name = "Apple",
                DefaultQuantity = 1
            };

            //Act
            var createdResponse = await _controller.CreateProduct(testItem);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }

        [Fact]
        public async void CreateProduct_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            //Arrange
            var testItem = new ProductForCreationDto
            {
                Name = "Apple",
                DefaultQuantity = 1
            };

            //Act
            var createdResponse = await _controller.CreateProduct(testItem) as CreatedAtRouteResult;
            var item = createdResponse?.Value as ProductDto;

            //Assert
            Assert.IsType<ProductDto>(item);
            Assert.Equal(testItem.Name, item?.Name);
        }

        [Fact]
        public async void GetProductCollection_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var testIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            //Act
            var notFoundResult = await _controller.GetProductCollection(testIds);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetProductCollection_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var testIds = new List<Guid>
            {
                new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4"),
                new Guid("9f8bede4-2052-486e-b41e-bd29f64d0bd6")
            };

            //Act
            var okResult = await _controller.GetProductCollection(testIds);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetProductCollection_ExistingGuidPassed_ReturnsRightItems()
        {
            //Arrange
            var testIds = new List<Guid>
            {
                new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4"),
                new Guid("9f8bede4-2052-486e-b41e-bd29f64d0bd6")
            };

            //Act
            var okResult = await _controller.GetProductCollection(testIds) as OkObjectResult;

            //Assert
            Assert.IsType<List<ProductDto>>(okResult?.Value);
            Assert.Equal(testIds, ((okResult?.Value as List<ProductDto>)!)
                .Select(i => i.Id).ToList());
        }

        [Fact]
        public async void CreateProductCollection_InvalidObjectPassed_ReturnsBadRequest()
        {
            //Arrange
            IEnumerable<ProductForCreationDto>? testItems = null;
            _controller.ModelState.AddModelError("productCollection", "null");

            //Act
            var badResponse = await _controller.CreateProductCollection(testItems);

            //Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void CreateProductCollection_ValidObjectPassed_ReturnsCreatedResponse()
        {
            //Arrange
            var testItems = new List<ProductForCreationDto>
            {
                new ProductForCreationDto
                {
                    Name = "Apple",
                    DefaultQuantity = 1
                },
                new ProductForCreationDto
                {
                    Name = "Eggs",
                    DefaultQuantity = 10
                }
            };

            //Act
            var createdResponse = await _controller.CreateProductCollection(testItems);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }

        [Fact]
        public async void CreateProductCollection_ReturnedResponseHasCreatedItems()
        {
            //Arrange
            var testItems = new List<ProductForCreationDto>
            {
                new ProductForCreationDto
                {
                    Name = "Apple",
                    DefaultQuantity = 1
                },
                new ProductForCreationDto
                {
                    Name = "Eggs",
                    DefaultQuantity = 10
                }
            };

            //Act
            var createdResponse = await _controller.CreateProductCollection(testItems)
                as CreatedAtRouteResult;
            var item = createdResponse?.Value as List<ProductDto>;

            //Assert
            Assert.IsType<List<ProductDto>>(item);
            Assert.Equal(testItems.Select(i => i.Name).ToList(),
                item!.Select(i => i.Name).ToList());
        }

        [Fact]
        public async void DeleteProduct_NotExistingGuidPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingGuid = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", notExistingGuid}
                },
                _controller
            );

            // Act
            await _validateProductExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void DeleteProduct_ExistingGuidPasses_ReturnsNoContentResult()
        {
            // Arrange
            var existingGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", existingGuid}
                },
                _controller
            );

            // Act
            await _validateProductExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var noContentResponse = await _controller.DeleteProduct(existingGuid);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse);
        }

        [Fact]
        public async void DeleteProduct_ExistingGuidPasses_RemovesOneItem()
        {
            // Arrange
            var existingGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", existingGuid}
                },
                _controller
            );

            // Act
            await _validateProductExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            await _controller.DeleteProduct(existingGuid);

            // Assert
            Assert.Equal(4, (await _repository.Product.GetAllProductsAsync(false))
                .ToList().Count());
        }

        [Fact]
        public async void UpdateProduct_NotExistingGuidPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingGuid = Guid.NewGuid();
            var productForUpdateDto = new ProductForUpdateDto
            {
                Name = "Apple",
                DefaultQuantity = 5
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", notExistingGuid },
                    {"productForUpdateDto", productForUpdateDto }
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateProductExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void UpdateProduct_ExistingGuidPasses_ReturnsNoContentResult()
        {
            // Arrange
            var existingGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");
            var productForUpdateDto = new ProductForUpdateDto
            {
                Name = "Apple",
                DefaultQuantity = 5
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", existingGuid},
                    {"productForUpdateDto", productForUpdateDto}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateProductExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var noContentResponse = await _controller.UpdateProduct(existingGuid,
                productForUpdateDto);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse);
        }

        [Fact]
        public async void UpdateProduct_ExistingGuidPasses_UpdateOneItem()
        {
            //Arrange
            var existingGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");
            var productForUpdateDto = new ProductForUpdateDto
            {
                Name = "Apple",
                DefaultQuantity = 5
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", existingGuid},
                    {"productForUpdateDto", productForUpdateDto}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateProductExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            await _controller.UpdateProduct(existingGuid, productForUpdateDto);

            //Assert
            Assert.Equal((await _repository.Product.GetProductAsync(existingGuid, false))
                .DefaultQuantity, productForUpdateDto.DefaultQuantity);
        }
}