namespace ContractManagement.WebAPI.Controllers;

/// <summary>
/// ContractCancellation command controller.
/// </summary>
public partial class CommandController
{  
    [HttpPost("cancelcontract")]
    public async Task<IActionResult> CancelContract(
        [FromBody] CancelContract command,
        [FromServices] ICommandHandler<CancelContract> commandHandler) => 
            await HandleCommand(command, commandHandler);
}
