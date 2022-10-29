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
using Microsoft.AspNetCore.Routing;
using Moq;

namespace FridgeServerTest.Tests.UnitTests
{
    public class FridgeControllerUnitTests
    {
        private readonly FridgeController _controller;
        private readonly RepositoryManagerMock _repository;
        private readonly Mock<ActionExecutionDelegate> _actionExecutionDelegate;
        private readonly ActionContext _actionContext;
        private readonly ValidationFilterAttribute _validationFilterAttribute;
        private readonly ValidateFridgeExistsAttribute _validateFridgeExistsAttribute;

        public FridgeControllerUnitTests()
        {
            _repository = new RepositoryManagerMock();
            var loggerManagerMock = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var httpContext = new DefaultHttpContext();
            _controller = new FridgeController(loggerManagerMock.Object, _repository, mapper)
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
            _validateFridgeExistsAttribute = new ValidateFridgeExistsAttribute(
                _repository, loggerManagerMock.Object);
        }

        [Fact]
        public async void GetFridges_WhenCalled_ReturnsAllFridges()
        {
            //Act
            var okResult = await _controller.GetFridges() as OkObjectResult;

            //Assert
            var items = Assert.IsType<List<FridgeDto>>(okResult?.Value);
            Assert.Equal(3, items.Count());
        }

        [Fact]
        public async void GetFridge_UnknownGuidPassed_ReturnsNotFoundResult()
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
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetFridge_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

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
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var okResult = await _controller.GetFridge(testGuid);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetFridge_ExistingGuidPassed_ReturnsRightItem()
        {
            //Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

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
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var okResult = await _controller.GetFridge(testGuid) as OkObjectResult;

            //Assert
            Assert.IsType<FridgeDto>(okResult?.Value);
            Assert.Equal(testGuid, ((okResult?.Value as FridgeDto)!).Id);
        }

        [Fact]
        public void CreateFridge_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var ownerNameMissingItem = new FridgeForCreationDto
            {
                Name = "Electrolux",
                OwnerName = "James"
            };

            _actionContext.ModelState.AddModelError("OwnerName", "Required");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"FridgeForCreationDto", ownerNameMissingItem }
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
        public async void CreateFridge_ValidObjectPassed_ReturnsCreatedResponse()
        {
            //Arrange
            var testItem = new FridgeForCreationDto
            {
                Name = "Electrolux",
                OwnerName = "James"
            };

            //Act
            var createdResponse = await _controller.CreateFridge(testItem);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }

        [Fact]
        public async void CreateFridge_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            //Arrange
            var testItem = new FridgeForCreationDto
            {
                Name = "Electrolux",
                OwnerName = "James"
            };

            //Act
            var createdResponse = await _controller.CreateFridge(testItem) as CreatedAtRouteResult;
            var item = createdResponse?.Value as FridgeDto;

            //Assert
            Assert.IsType<FridgeDto>(item);
            Assert.Equal(testItem.Name, item?.Name);
        }

        [Fact]
        public async void GetFridgeCollection_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var testIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            //Act
            var notFoundResult = await _controller.GetFridgeCollection(testIds);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetFridgeCollection_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var testIds = new List<Guid>
            {
                new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad")
            };

            //Act
            var okResult = await _controller.GetFridgeCollection(testIds);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetFridgeCollection_ExistingGuidPassed_ReturnsRightItems()
        {
            //Arrange
            var testIds = new List<Guid>
            {
                new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad")
            };

            //Act
            var okResult = await _controller.GetFridgeCollection(testIds) as OkObjectResult;

            //Assert
            Assert.IsType<List<FridgeDto>>(okResult?.Value);
            Assert.Equal(testIds, ((okResult?.Value as List<FridgeDto>)!)
                .Select(i => i.Id).ToList());
        }

        [Fact]
        public async void CreateFridgeCollection_InvalidObjectPassed_ReturnsBadRequest()
        {
            //Arrange
            IEnumerable<FridgeForCreationDto>? testItems = null;
            _controller.ModelState.AddModelError("fridgeCollection", "null");

            //Act
            var badResponse = await _controller.CreateFridgeCollection(testItems);

            //Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void CreateFridgeCollection_ValidObjectPassed_ReturnsCreatedResponse()
        {
            //Arrange
            var testItems = new List<FridgeForCreationDto>
            {
                new FridgeForCreationDto
                {
                    Name = "Electrolux",
                    OwnerName = "James"
                },
                new FridgeForCreationDto
                {
                    Name = "ATLANT",
                    OwnerName = "Ivan"
                }
            };

            //Act
            var createdResponse = await _controller.CreateFridgeCollection(testItems);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }

        [Fact]
        public async void CreateFridgeCollection_ReturnedResponseHasCreatedItems()
        {
            //Arrange
            var testItems = new List<FridgeForCreationDto>
            {
                new FridgeForCreationDto
                {
                    Name = "Electrolux",
                    OwnerName = "James"
                },
                new FridgeForCreationDto
                {
                    Name = "ATLANT",
                    OwnerName = "Ivan"
                }
            };

            //Act
            var createdResponse = await _controller.CreateFridgeCollection(testItems)
                as CreatedAtRouteResult;
            var item = createdResponse?.Value as List<FridgeDto>;

            //Assert
            Assert.IsType<List<FridgeDto>>(item);
            Assert.Equal(testItems.Select(i => i.Name).ToList(),
                item!.Select(i => i.Name).ToList());
        }

        [Fact]
        public async void DeleteFridge_NotExistingGuidPassed_ReturnsNotFoundResponse()
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
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void DeleteFridge_ExistingGuidPasses_ReturnsNoContentResult()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

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
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            var noContentResponse = await _controller.DeleteFridge(existingGuid);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse);
        }

        [Fact]
        public async void DeleteFridge_ExistingGuidPasses_RemovesOneItem()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

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
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(actionExecutingContext,
                _actionExecutionDelegate.Object);
            await _controller.DeleteFridge(existingGuid);

            // Assert
            Assert.Equal(2, (await _repository.Fridge.GetAllFridgesAsync(false))
                .ToList().Count());
        }

        [Fact]
        public async void UpdateFridge_NotExistingGuidPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingGuid = Guid.NewGuid();
            var fridgeForUpdateDto = new FridgeForUpdateDto
            {
                Name = "LG",
                OwnerName = "Robert"
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", notExistingGuid },
                    {"fridgeForUpdateDto", fridgeForUpdateDto }
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void UpdateFridge_ExistingGuidPasses_ReturnsNoContentResult()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var fridgeForUpdateDto = new FridgeForUpdateDto
            {
                Name = "LG",
                OwnerName = "Robert"
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", existingGuid},
                    {"fridgeForUpdateDto", fridgeForUpdateDto}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var noContentResponse = await _controller.UpdateFridge(existingGuid,
                fridgeForUpdateDto);

            // Assert
            Assert.IsType<NoContentResult>(noContentResponse);
        }

        [Fact]
        public async void UpdateFridge_ExistingGuidPasses_UpdateOneItem()
        {
            //Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var fridgeForUpdateDto = new FridgeForUpdateDto
            {
                Name = "LG",
                OwnerName = "Robert"
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"id", existingGuid},
                    {"fridgeForUpdateDto", fridgeForUpdateDto}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            await _controller.UpdateFridge(existingGuid, fridgeForUpdateDto);

            //Assert
            Assert.Equal((await _repository.Fridge.GetFridgeAsync(existingGuid, false))
                .OwnerName, fridgeForUpdateDto.OwnerName);
        }
    }
}
