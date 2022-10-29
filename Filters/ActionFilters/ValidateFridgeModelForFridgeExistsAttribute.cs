using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.ActionFilters
{
    public class ValidateFridgeModelForFridgeExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _loggerManager;

        public ValidateFridgeModelForFridgeExistsAttribute(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _loggerManager = loggerManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH"));

            var fridgeId = (Guid)context.ActionArguments["fridgeId"];
            var fridge = await _repositoryManager.Fridge.GetFridgeAsync(fridgeId, false);

            if (fridge == null)
            {
                _loggerManager.LogInfo($"Fridge with id: {fridgeId} doesn't exist in the" +
                    $" database");
                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["id"];
            var fridgeModel = await _repositoryManager.FridgeModel
                .GetFridgeModelAsync(fridgeId, id, trackChanges);

            if (fridgeModel == null)
            {
                _loggerManager.LogInfo($"FridgeModel with id {id} doesn't exist in the" +
                    $" database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("fridgeModel", fridgeModel);
                await next();
            }
        }
    }
}
