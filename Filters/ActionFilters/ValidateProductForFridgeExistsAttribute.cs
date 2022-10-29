using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.ActionFilters;

public class ValidateProductForFridgeExistsAttribute : IAsyncActionFilter
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;

    public ValidateProductForFridgeExistsAttribute(IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var method = context.HttpContext.Request.Method;
        var trackChanges = method.Equals("PUT");

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
        var product = await _repositoryManager.Product.GetProductAsync(id, trackChanges);
        
        if (product == null)
        {
            _loggerManager.LogInfo($"Product with id {id} doesn't exist in the" +
                                   $" database.");
            context.Result = new NotFoundResult();
            return;
        }

        context.HttpContext.Items.Add("product", product);
        await next();
    }
}