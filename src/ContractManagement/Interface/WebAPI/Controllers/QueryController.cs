namespace ContractManagement.WebAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("contractmanagement/query")]
public class QueryController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ServiceDbContext _dbContext;

    public QueryController(ServiceDbContext dbContext, ILogger<QueryController> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet("contracts")]
    public ActionResult GetContracts()
    {
        return new JsonResult(_dbContext.Contracts.ToList());
    }

    [HttpGet("contracts/{contractNumber}")]
    public ActionResult GetContracts(string contractNumber)
    {
        var contract = _dbContext.Contracts.FirstOrDefault(c => c.ContractNumber == contractNumber);
        return contract != null ?
            new JsonResult(contract) :
            NotFound();
    }    
}
