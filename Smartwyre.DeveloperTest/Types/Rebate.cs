using System;

namespace Smartwyre.DeveloperTest.Types;

public class Rebate
{
    public Guid Identifier { get; set; }
    public IncentiveType Incentive { get; set; }
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
}
