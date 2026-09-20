using Microsoft.AspNetCore.Mvc;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner.Controllers;

[ApiController]
[Route("rebates")]
public class RebatesController(IRebateService rebateService) : ControllerBase
{
    [HttpPost("calculate", Name = "CalculateRebate")]
    public ActionResult<CalculateRebateResult> Calculate([FromBody] CalculateRebateRequest request)
    {
        return Ok(rebateService.Calculate(request));
    }
}
