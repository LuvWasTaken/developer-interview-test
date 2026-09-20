using System;

namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResponse
{
    public bool Success { get; init; }
    public Guid? CalculationIdentifier { get; init; }
}
