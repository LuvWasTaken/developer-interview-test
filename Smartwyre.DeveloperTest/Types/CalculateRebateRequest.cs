using System;

namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateRequest
{
    public Guid RebateIdentifier { get; set; }
    public Guid ProductIdentifier { get; set; }
    public decimal Volume { get; set; }
}
