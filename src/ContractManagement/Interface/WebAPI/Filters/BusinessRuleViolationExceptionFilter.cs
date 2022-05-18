using System.Text;

namespace ContractManagement.WebApi.Filters;

public class BusinessRuleViolationExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is BusinessRuleViolationException exception)
        {
            var message = new StringBuilder();
            message.AppendLine(exception.Message);
            foreach (var detail in exception.Details)
            {
                message.AppendLine($"- {detail}");
            }
            context.Result = new ObjectResult(message.ToString())
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            context.ExceptionHandled = true;
        }
    }
}