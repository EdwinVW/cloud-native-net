namespace ContractManagement.WebAPI.Controllers;

/// <summary>
/// ContractMaintenance command controller.
/// </summary>
public partial class CommandController
{  
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
}
