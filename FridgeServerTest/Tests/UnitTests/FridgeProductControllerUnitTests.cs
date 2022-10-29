using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Filters.ActionFilters;
using FridgeServer.AutoMapperProfile;
using FridgeServer.Controllers;
using FridgeServerTest.MoqObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace FridgeServerTest.Tests.UnitTests;

public class FridgeProductControllerUnitTests
{
        private readonly FridgeProductController _controller;
        private readonly RepositoryManagerMock _repository;
        private readonly Mock<ActionExecutionDelegate> _actionExecutionDelegate;
        private readonly ActionContext _actionContext;
        private readonly ValidationFilterAttribute _validationFilterAttribute;
        private readonly ValidateProductForFridgeExistsAttribute _validateProductForFridgeExistsAttribute;

        public FridgeProductControllerUnitTests()
        {
            _repository = new RepositoryManagerMock();
            var loggerManagerMock = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var httpContext = new DefaultHttpContext();
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            _controller = new FridgeProductController(loggerManagerMock.Object, _repository, mapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
                ObjectValidator = objectValidator.Object
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
            _validateProductForFridgeExistsAttribute =
                new ValidateProductForFridgeExistsAttribute(_repository,
                loggerManagerMock.Object);
        }

        [Fact]
        public async void GetProductsForFridge_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingGuidFridgeId = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingGuidFridgeId}
                },
                _controller
            );

            // Act
            var notFoundResult = await _controller.GetProductsForFridge(notExistingGuidFridgeId);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetProductsForFridge_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var existingGuidFridgeId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingGuidFridgeId}
                },
                _controller
            );

            // Act
            var okResult = await _controller.GetProductsForFridge(existingGuidFridgeId);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetProductForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingGuidFridgeId = Guid.NewGuid();
            var notExistingGuidId = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingGuidFridgeId},
                    {"id", notExistingGuidId}
                },
                _controller
            );

            // Act
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetProductForFridge_UnknownProductGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var notExistingGuidId = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", notExistingGuidId}
                },
                _controller
            );

            // Act
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetProductForFridge_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingProductGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingProductGuid}
                },
                _controller
            );

            // Act
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var okResult = await _controller.GetProductForFridge(existingFridgeGuid,
                existingProductGuid);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async void GetProductForFridge_ExistingGuidPassed_ReturnsRightItem()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingProductGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingProductGuid}
                },
                _controller
            );

            // Act
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var okResult = await _controller.GetProductForFridge(existingFridgeGuid,
                existingProductGuid) as OkObjectResult;

            //Assert
            Assert.IsType<ProductDto>(okResult?.Value);
            Assert.Equal(existingProductGuid, ((okResult.Value as ProductDto)!).Id);
        }

        [Fact]
        public void CreateProductForFridge_ObjectPassedIsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var notExistingGuid = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingGuid},
                    {"id", notExistingGuid},
                    {"fridgeProductForCreationDto", null}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var badRequestResult = actionExecutingContext.Result;

            //Arrange
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public void CreateProductForFridge_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var quantityMissingItem = new FridgeProductForCreationDto();
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingProductGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");
            _actionContext.ModelState.AddModelError("Quantity", "Required");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingProductGuid},
                    {"fridgeProductForCreationDto", quantityMissingItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var unprocessableEntityResult = actionExecutingContext.Result;

            //Arrange
            Assert.IsType<UnprocessableEntityObjectResult>(unprocessableEntityResult);
        }

        [Fact]
        public async void CreateProductForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var testItem = new FridgeProductForCreationDto
            {
                Quantity = 1
            };
            var notExistingGuid = Guid.NewGuid();
            var existingProductGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingGuid},
                    {"id", existingProductGuid},
                    {"fridgeProductForCreationDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Arrange
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void CreateProductForFridge_ValidObjectPassed_ReturnsCreateAtRouteResponse()
        {
            // Arrange
            var testItem = new FridgeProductForCreationDto
            {
                Quantity = 1
            };
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingProductGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingProductGuid},
                    {"fridgeProductForCreationDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var createAtRouteResult = await _controller.CreateProductForFridge(existingFridgeGuid,
                existingProductGuid, testItem);

            //Arrange
            Assert.IsType<CreatedAtRouteResult>(createAtRouteResult);
        }

        [Fact]
        public async void CreateProductForFridge_ValidObjectPassed_ReturnsResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new FridgeProductForCreationDto
            {
                Quantity = 1
            };
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingProductGuid = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingProductGuid},
                    {"fridgeModelForCreationDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateProductForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var createAtRouteResult = await _controller.CreateProductForFridge(existingFridgeGuid,
                existingProductGuid, testItem) as CreatedAtRouteResult;
            var item = createAtRouteResult?.Value as ProductDto;

            //Arrange
            Assert.IsType<ProductDto>(createAtRouteResult?.Value);
            Assert.Equal(existingProductGuid, item?.Id);
        }

        [Fact]
        public async void DeleteProductForFridge_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingFridgeProductGuid = Guid.NewGuid();

            // Act;
            var notFoundResult = await _controller.DeleteProductForFridge(notExistingFridgeProductGuid);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void DeleteProductForFridge_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            var existingFridgeProductGuid = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d");

            // Act;
            var noContentResult = await _controller.DeleteProductForFridge(existingFridgeProductGuid);

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async void DeleteProductForFridge_ExistingGuidPasses_RemovesOneItem()
        {
            //Arrange
            var existingFridgeProductGuid = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d");

            // Act;
            var noContentResult = await _controller.DeleteProductForFridge(existingFridgeProductGuid);

            // Assert
            Assert.Null(await _repository.FridgeProduct.GetFridgeProductAsync(existingFridgeProductGuid,
                false));
        }

        [Fact]
        public void UpdateProductForFridge_ObjectPassedIsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var existingFridgeProductGuid = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeProductId", existingFridgeProductGuid},
                    {"fridgeProductForUpdateDto", null}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var badRequestResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public void UpdateProductForFridge_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var quantityMissingItem = new FridgeProductForUpdateDto();
            var existingFridgeProductGuid = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d");
            _actionContext.ModelState.AddModelError("Quantity", "Required");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeProductId", existingFridgeProductGuid},
                    {"fridgeProductForUpdateDto", quantityMissingItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var unprocessableEntityResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(unprocessableEntityResult);
        }

        [Fact]
        public async void UpdateProductForFridge_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var testItem = new FridgeProductForUpdateDto
            {
                Quantity = 5
            };
            var notExistingFridgeProductGuid = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeProductId", notExistingFridgeProductGuid},
                    {"fridgeProductForUpdateDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var notFoundResult = await _controller.UpdateProductForFridge(notExistingFridgeProductGuid,
                testItem);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void UpdateProductForFridge_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            var testItem = new FridgeProductForUpdateDto
            {
                Quantity = 5
            };
            var existingFridgeProductGuid = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeProductId", existingFridgeProductGuid},
                    {"fridgeProductForUpdateDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var noContentResult = await _controller.UpdateProductForFridge(existingFridgeProductGuid,
                testItem);

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async void UpdateProductForFridge_ExistingGuidPasses_UpdateOneItem()
        {
            // Arrange
            var testItem = new FridgeProductForUpdateDto
            {
                Quantity = 5
            };
            var existingFridgeProductGuid = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeProductId", existingFridgeProductGuid},
                    {"fridgeProductForDeleteDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _controller.UpdateProductForFridge(existingFridgeProductGuid, testItem);

            //Assert
            Assert.Equal((await _repository.FridgeProduct
                .GetFridgeProductAsync(existingFridgeProductGuid, false)).Quantity,
                testItem.Quantity);
        }
}