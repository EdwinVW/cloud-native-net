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
            foreach (var violationMessage in exception.Violations)
            {
                message.AppendLine($"- {violationMessage}");
            }
            context.Result = new ObjectResult(message.ToString())
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            context.ExceptionHandled = true;
        }
    }
}