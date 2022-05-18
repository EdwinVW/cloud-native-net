using Domain.Common;

namespace ContractManagement.WebAPI.Controllers;

/// <summary>
/// ContractManagement controller.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("contractmanagement/command")]
public class CommandController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CommandController(IUnitOfWork unitOfWork, ILogger<CommandController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// This is a method that only exists during the transition from RegisterContract 
    /// to RegisterContractV2. As soon as no clients use RegisterContract anymore, this 
    /// method (and the command) can be removed from the codebase.
    /// </summary>
    [Obsolete]
    [HttpPost("registercontract")]
    public IActionResult HandleRegisterContract(
        [FromBody] RegisterContract command)
    {
        if (ModelState.IsValid)
        {
            // Map command to latest version
            var commandV2 = RegisterContractV2.CreateFrom(command);

            _logger.LogInformation("Mapped {@command} to {@mappedCommand}", command.Type, commandV2.Type);

            // Handle event (call latest version)
            return RedirectToActionPreserveMethod(actionName: "HandleRegisterContractV2", routeValues: commandV2);
        }

        return BadRequest();
    }

    [HttpPost("registercontractv2", Name = "HandleRegisterContractV2")]
    public async Task<IActionResult> HandleRegisterContractV2(
        [FromBody] RegisterContractV2 command,
        [FromServices] ICommandHandler<RegisterContractV2> commandHandler) => 
            await HandleCommand(command, commandHandler);

    [HttpPost("changecontractamount")]
    public async Task<IActionResult> ChangeContractAmount(
        [FromBody] ChangeContractAmount command,
        [FromServices] ICommandHandler<ChangeContractAmount> commandHandler) => 
            await HandleCommand(command, commandHandler);

    [HttpPost("changecontractterm")]
    public async Task<IActionResult> ChangeContractTerm(
        [FromBody] ChangeContractTerm command,
        [FromServices] ICommandHandler<ChangeContractTerm> commandHandler) => 
            await HandleCommand(command, commandHandler);            

    [HttpPost("cancelcontract")]
    public async Task<IActionResult> CancelContract(
        [FromBody] CancelContract command,
        [FromServices] ICommandHandler<CancelContract> commandHandler) => 
            await HandleCommand(command, commandHandler);    

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
