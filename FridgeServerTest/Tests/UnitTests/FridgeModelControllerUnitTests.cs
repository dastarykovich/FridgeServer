using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Filters.ActionFilters;
using FridgeServer.AutoMapperProfile;
using FridgeServer.Controllers;
using FridgeServer.Utility;
using FridgeServerTest.MoqObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Moq;
using Repository.DataShaping;

namespace FridgeServerTest.Tests.UnitTests
{
    public class FridgeModelControllerUnitTests
    {
        private readonly FridgeModelController _controller;
        private readonly RepositoryManagerMock _repository;
        private readonly Mock<ActionExecutionDelegate> _actionExecutionDelegate;
        private readonly ActionContext _actionContext;
        private readonly ValidationFilterAttribute _validationFilterAttribute;
        private readonly ValidateFridgeModelForFridgeExistsAttribute _validateFridgeModelForFridgeExistsAttribute;
        private readonly ValidateMediaTypeAttribute _validateMediaTypeAttribute;

        public FridgeModelControllerUnitTests()
        {
            _repository = new RepositoryManagerMock();
            var loggerManagerMock = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var linkGeneratorMock = Mock.Of<LinkGenerator>();
            var fridgeModelLinksMock = new FridgeModelLinks(linkGeneratorMock,
                new DataShaper<FridgeModelDto>());

            var httpContext = new DefaultHttpContext();
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            _controller = new FridgeModelController(loggerManagerMock.Object,
                _repository, mapper, fridgeModelLinksMock)
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
            _validateFridgeModelForFridgeExistsAttribute =
                new ValidateFridgeModelForFridgeExistsAttribute(_repository,
                loggerManagerMock.Object);
            _validateMediaTypeAttribute = new ValidateMediaTypeAttribute();
        }

        [Fact]
        public async void GetFridgeModelsForFridge_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingGuidFridgeId = Guid.NewGuid();
            var fridgeModelParameters = new FridgeModelParameters();
            _actionContext.HttpContext.Request.Headers.Add("Accept",
                "application/vnd.fridge.hateoas+json");

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
            _validateMediaTypeAttribute.OnActionExecuting(actionExecutingContext);
            var notFoundResult = await _controller.GetFridgeModelsForFridge(notExistingGuidFridgeId,
                fridgeModelParameters);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetFridgeModelsForFridge_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var existingGuidFridgeId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var fridgeModelParameters = new FridgeModelParameters();
            _actionContext.HttpContext.Request.Headers.Add("Accept",
                "application/vnd.fridge.hateoas+json");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingGuidFridgeId},
                    {"fridgeModelParameters", fridgeModelParameters}
                },
                _controller
            );

            // Act
            _validateMediaTypeAttribute.OnActionExecuting(actionExecutingContext);
            var okResult = await _controller.GetFridgeModelsForFridge(existingGuidFridgeId,
                fridgeModelParameters);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetFridgeModelForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
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
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetFridgeModelForFridge_UnknownFridgeModelGuidPassed_ReturnsNotFoundResult()
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
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetFridgeModelForFridge_ExistingGuidPassed_ReturnsOkResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var okResult = await _controller.GetFridgeModelForFridge(existingFridgeGuid,
                existingFridgeModelGuid);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async void GetFridgeModelForFridge_ExistingGuidPassed_ReturnsRightItem()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var okResult = await _controller.GetFridgeModelForFridge(existingFridgeGuid,
                existingFridgeModelGuid) as OkObjectResult;

            //Assert
            Assert.IsType<FridgeModelDto>(okResult?.Value);
            Assert.Equal(existingFridgeGuid, ((okResult?.Value as FridgeModelDto)!).FridgeId);
            Assert.Equal(existingFridgeModelGuid, ((okResult.Value as FridgeModelDto)!).Id);
        }

        [Fact]
        public void CreateFridgeModelForFridge_ObjectPassedIsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var notExistingGuid = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingGuid},
                    {"fridgeModelForCreationDto", null}
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
        public void CreateFridgeModelForFridge_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var nameMissingItem = new FridgeModelForCreationDto
            {
                Year = 2021
            };
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            _actionContext.ModelState.AddModelError("Name", "Required");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingGuid},
                    {"fridgeModelForCreationDto", nameMissingItem}
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
        public async void CreateFridgeModelForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var testItem = new FridgeModelForCreationDto
            {
                Name = "GD-3HFV5DE",
                Year = 2021
            };
            var notExistingGuid = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingGuid},
                    {"fridgeModelForCreationDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var notFoundResult = await _controller.CreateFridgeModelForFridge(notExistingGuid,
                testItem);

            //Arrange
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void CreateFridgeModelForFridge_ValidObjectPassed_ReturnsCreateAtRouteResponse()
        {
            // Arrange
            var testItem = new FridgeModelForCreationDto
            {
                Name = "GD-3HFV5DE",
                Year = 2021
            };
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingGuid},
                    {"fridgeModelForCreationDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var createAtRouteResult = await _controller.CreateFridgeModelForFridge(existingGuid,
                testItem);

            //Arrange
            Assert.IsType<CreatedAtRouteResult>(createAtRouteResult);
        }

        [Fact]
        public async void CreateFridgeModelForFridge_ValidObjectPassed_ReturnsResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new FridgeModelForCreationDto
            {
                Name = "GD-3HFV5DE",
                Year = 2021
            };
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingGuid},
                    {"fridgeModelForCreationDto", testItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            var createAtRouteResult = await _controller.CreateFridgeModelForFridge(existingGuid,
                testItem) as CreatedAtRouteResult;
            var item = createAtRouteResult?.Value as FridgeModelDto;

            //Arrange
            Assert.IsType<FridgeModelDto>(createAtRouteResult?.Value);
            Assert.Equal(testItem.Name, item?.Name);
        }

        [Fact]
        public async void DeleteFridgeModelForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingFridgeGuid = Guid.NewGuid();
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingFridgeGuid},
                    {"id", existingFridgeModelGuid}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void DeleteFridgeModelForFridge_UnknownFridgeModelGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var notExistingFridgeModelGuid = Guid.NewGuid();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", notExistingFridgeModelGuid}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void DeleteFridgeModelForFridge_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var noContentResult = await _controller.DeleteFridgeModelForFridge(existingFridgeGuid,
                existingFridgeModelGuid);

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async void DeleteFridgeModelForFridge_ExistingGuidPasses_RemovesOneItem()
        {
            // Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            await _controller.DeleteFridgeModelForFridge(existingFridgeGuid, existingFridgeModelGuid);


            // Assert
            Assert.Equal((await _repository.FridgeModel
                    .GetFridgeModelAsync(existingFridgeGuid, existingFridgeModelGuid, false))
                , null);
        }

        [Fact]
        public async void UpdateFridgeModelForFridge_ObjectPassedIsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"fridgeForUpdateDto", null}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var badRequestResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public async void UpdateFridgeModelForFridge_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var nameMissingItem = new FridgeModelForUpdateDto
            {
                Year = 2021
            };
            _actionContext.ModelState.AddModelError("Name", "Required");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"fridgeForUpdateDto", nameMissingItem}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var unprocessableEntityResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(unprocessableEntityResult);
        }

        [Fact]
        public async void UpdateFridgeModelForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingFridgeGuid = Guid.NewGuid();
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var testFridgeModel = new FridgeModelForUpdateDto
            {
                Name = "testName",
                Year = 2021
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"fridgeForUpdateDto", testFridgeModel}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void UpdateFridgeModelForFridge_UnknownFridgeModelGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var notExistingFridgeModelGuid = Guid.NewGuid();
            var testFridgeModel = new FridgeModelForUpdateDto
            {
                Name = "testName",
                Year = 2021
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", notExistingFridgeModelGuid},
                    {"fridgeForUpdateDto", testFridgeModel}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void UpdateFridgeModelForFridge_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var testFridgeModel = new FridgeModelForUpdateDto
            {
                Name = "testName",
                Year = 2021
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"fridgeForUpdateDto", testFridgeModel}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var noContentResult = await _controller.UpdateFridgeModelForFridge(existingFridgeGuid,
                existingFridgeModelGuid, testFridgeModel);

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async void UpdateFridgeModelForFridge_ExistingGuidPasses_UpdateOneItem()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var fridgeModelParameters = new FridgeModelParameters();
            var testFridgeModel = new FridgeModelForUpdateDto
            {
                Name = "testName",
                Year = 2021
            };

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"fridgeForUpdateDto", testFridgeModel}
                },
                _controller
            );

            // Act
            _validationFilterAttribute.OnActionExecuting(actionExecutingContext);
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            await _controller
                .UpdateFridgeModelForFridge(existingFridgeGuid, existingFridgeModelGuid,
                    testFridgeModel);

            //Assert
            Assert.Equal((await _repository.FridgeModel.GetFridgeModelsAsync(existingFridgeGuid,
                fridgeModelParameters, false))
                .FirstOrDefault(i => i.Id.Equals(existingFridgeModelGuid))
                ?.Name,
                testFridgeModel.Name);
        }

        [Fact]
        public async void PartiallyUpdateFridgeModelForFridge_ObjectPassedIsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"patchDoc", null}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var badRequestResult = await _controller
                .PartiallyUpdateFridgeModelForFridge(existingFridgeGuid, existingFridgeModelGuid,
                null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public async void PartiallyUpdateFridgeModelForFridge_UnknownFridgeGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var notExistingFridgeGuid = Guid.NewGuid();
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var testPatchDoc = new JsonPatchDocument<FridgeModelForUpdateDto>();
            testPatchDoc.Replace(i => i.Name, value: "testName");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", notExistingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"patchDoc", testPatchDoc}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void PartiallyUpdateFridgeModelForFridge_UnknownFridgeModelGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var notExistingFridgeModelGuid = Guid.NewGuid();
            var testPatchDoc = new JsonPatchDocument<FridgeModelForUpdateDto>();
            testPatchDoc.Replace(i => i.Name, value: "testName");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", notExistingFridgeModelGuid},
                    {"patchDoc", testPatchDoc}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var notFoundResult = actionExecutingContext.Result;

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void PartiallyUpdateFridgeModelForFridge_InvalidObjectPassed_ReturnsUnprocessableEntity()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var testPatchDoc = new JsonPatchDocument<FridgeModelForUpdateDto>();
            testPatchDoc.Replace(i => i.Name, value: null);
            _controller.ModelState.AddModelError("Name", "null");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"patchDoc", testPatchDoc}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var unprocessableEntityResult = await _controller
                .PartiallyUpdateFridgeModelForFridge(existingFridgeGuid,
                existingFridgeModelGuid, testPatchDoc);

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(unprocessableEntityResult);
        }

        [Fact]
        public async void PartiallyUpdateFridgeModelForFridge_ExistingGuidPassed_ReturnsNoContentResult()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var testPatchDoc = new JsonPatchDocument<FridgeModelForUpdateDto>();
            testPatchDoc.Replace(i => i.Name, value: "testName");

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"patchDoc", testPatchDoc}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            var noContentResult = await _controller
                .PartiallyUpdateFridgeModelForFridge(existingFridgeGuid, existingFridgeModelGuid,
                testPatchDoc);

            //Assert
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async void PartiallyUpdateFridgeModelForFridge_ExistingGuidPassed_PartiallyUpdateOneItem()
        {
            //Arrange
            var existingFridgeGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var existingFridgeModelGuid = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1");
            var testPatchDoc = new JsonPatchDocument<FridgeModelForUpdateDto>();
            testPatchDoc.Replace(i => i.Name, value: "testName");
            var fridgeModelParameters = new FridgeModelParameters();

            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>
                {
                    {"fridgeId", existingFridgeGuid},
                    {"id", existingFridgeModelGuid},
                    {"patchDoc", testPatchDoc}
                },
                _controller
            );

            // Act
            await _validateFridgeModelForFridgeExistsAttribute.OnActionExecutionAsync(
                actionExecutingContext, _actionExecutionDelegate.Object);
            await _controller
                .PartiallyUpdateFridgeModelForFridge(existingFridgeGuid, existingFridgeModelGuid,
                    testPatchDoc);

            //Assert
            Assert.Equal("testName", (await _repository.FridgeModel.GetFridgeModelsAsync(existingFridgeGuid,
                fridgeModelParameters, false)).FirstOrDefault(i =>
                i.Id.Equals(existingFridgeModelGuid))
                ?.Name);
        }
    }
}
