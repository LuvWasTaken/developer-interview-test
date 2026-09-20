using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data.Repositories;

public class RebateRepository(RebateDbContext context) : IRebateRepository
{
    public Task<Rebate?> GetByIdentifierAsync(Guid identifier)
    {
        return context.Rebates.AsNoTracking().FirstOrDefaultAsync(rebate => rebate.Identifier == identifier);
    }

    public Task<RebateCalculation?> GetCalculationByIdentifierAsync(Guid identifier)
    {
        return context.RebateCalculations.AsNoTracking()
            .FirstOrDefaultAsync(calculation => calculation.Identifier == identifier);
    }

    public async Task<Guid> StoreCalculationResultAsync(Rebate rebate, decimal amount)
    {
        var calculation = new RebateCalculation
        {
            Identifier = Guid.NewGuid(),
            RebateIdentifier = rebate.Identifier,
            IncentiveType = rebate.Incentive,
            Amount = amount
        };

        context.RebateCalculations.Add(calculation);

        await context.SaveChangesAsync();

        return calculation.Identifier;
    }
}
