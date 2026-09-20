using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Smartwyre.DeveloperTest.Data.Repositories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner.Controllers;

[ApiController]
[Route("rebates")]
public class RebatesController(
    IRebateService rebateService,
    IRebateRepository rebateRepository,
    IProductRepository productRepository) : ControllerBase
{
    [HttpPost("calculate", Name = "CalculateRebate")]
    public async Task<Results<Ok<CalculateRebateResponse>, NotFound>> CalculateAsync(
        [FromBody] CalculateRebateRequest request)
    {
        var rebate = await rebateRepository.GetByIdentifierAsync(request.RebateIdentifier);
        var product = await productRepository.GetByIdentifierAsync(request.ProductIdentifier);

        if (rebate == null || product == null)
        {
            return TypedResults.NotFound();
        }

        var result = await rebateService.CalculateAsync(rebate, product, request.Volume);
        var response = new CalculateRebateResponse
        {
            Success = result.Success,
            CalculationIdentifier = result.CalculationIdentifier
        };

        return TypedResults.Ok(response);
    }

    [HttpGet("calculations/{identifier:guid}", Name = "GetRebateCalculation")]
    public async Task<Results<Ok<RebateCalculationResponse>, NotFound>> GetCalculationAsync(
        Guid identifier)
    {
        var calculation = await rebateRepository.GetCalculationByIdentifierAsync(identifier);
        if (calculation == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new RebateCalculationResponse
        {
            Identifier = calculation.Identifier,
            RebateIdentifier = calculation.RebateIdentifier,
            IncentiveType = calculation.IncentiveType,
            Amount = calculation.Amount
        });
    }
}
