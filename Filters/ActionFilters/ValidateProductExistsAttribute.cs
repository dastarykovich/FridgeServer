using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.ActionFilters;

public class ValidateProductExistsAttribute : IAsyncActionFilter
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;

    public ValidateProductExistsAttribute(IRepositoryManager repositoryManager,
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
        var product = await _repositoryManager.Product.GetProductAsync(id, trackChanges);

        if (product == null)
        {
            _loggerManager.LogInfo($"Product with id {id} doesn't not exist in the" +
                                   $" database.");
            context.Result = new NotFoundResult();
        }
        else
        {
            context.HttpContext.Items.Add("product", product);
            await next();
        }
    }
}