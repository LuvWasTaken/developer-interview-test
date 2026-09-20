using System;

namespace Smartwyre.DeveloperTest.Types;

public class RebateCalculationResponse
{
    public Guid Identifier { get; init; }
    public Guid RebateIdentifier { get; init; }
    public IncentiveType IncentiveType { get; init; }
    public decimal Amount { get; init; }
}
