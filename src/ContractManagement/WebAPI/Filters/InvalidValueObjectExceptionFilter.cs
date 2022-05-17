namespace ContractManagement.WebApi.Filters;

public class InvalidValueObjectExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is InvalidValueObjectException exception)
        {
            context.Result = new JsonResult(new { Message = exception.Message })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            context.ExceptionHandled = true;
        }
    }
}