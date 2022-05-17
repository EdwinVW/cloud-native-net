namespace ContractManagement.WebAPI.Controllers;

/// <summary>
/// ContractRegistration command controller.
/// </summary>
public partial class CommandController : ControllerBase
{
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
}
