using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.ActionFilters
{
    public class ValidateFridgeExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _loggerManager;

        public ValidateFridgeExistsAttribute(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _loggerManager = loggerManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            var fridge = await _repositoryManager.Fridge.GetFridgeAsync(id, trackChanges);

            if (fridge == null)
            {
                _loggerManager.LogInfo($"Fridge with id {id} doesn't not exist in the" +
                    $" database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("fridge", fridge);
                await next();
            }
        }
    }
}
