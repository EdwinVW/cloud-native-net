using Domain.Common;

namespace ContractManagement.WebAPI.Controllers;

/// <summary>
/// ContractManagement controller.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("contractmanagement/command")]
public partial class CommandController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CommandController(IUnitOfWork unitOfWork, ILogger<CommandController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    private async Task<IActionResult> HandleCommand<T>(
        Command command, ICommandHandler<T> commandHandler) where T : Command
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation(
                "Consume Command '{CommandType}'. Message: {@Message}.",
                command.Type,
                command);

            await commandHandler.HandleAsync((T)command);
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        return BadRequest();
    }
}
