namespace ContractManagement.WebApi.Filters;

public class ConcurrencyExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ConcurrencyException exception)
        {
            context.Result = new JsonResult(new { Message = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.Conflict
            };

            context.ExceptionHandled = true;
        }
    }
}