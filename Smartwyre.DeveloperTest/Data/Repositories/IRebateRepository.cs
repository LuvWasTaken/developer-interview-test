using System.Threading.Tasks;
using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data.Repositories;

public interface IRebateRepository
{
    Task<Rebate?> GetByIdentifierAsync(Guid identifier);
    Task<RebateCalculation?> GetCalculationByIdentifierAsync(Guid identifier);
    Task<Guid> StoreCalculationResultAsync(Rebate rebate, decimal amount);
}
