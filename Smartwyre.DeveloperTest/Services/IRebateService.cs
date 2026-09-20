using System.Threading;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IRebateService
{
    Task<CalculateRebateResult> CalculateAsync(Rebate rebate, Product product, decimal volume);
}
