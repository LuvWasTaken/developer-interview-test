using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Data.Repositories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    [Fact]
    public async Task CalculateAsync_FixedCashAmount_StoresRebateAmount()
    {
        var repository = new RecordingRebateRepository();
        var service = new RebateService(repository);
        var rebate = new Rebate
        {
            Identifier = Guid.NewGuid(),
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 25m
        };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        var result = await service.CalculateAsync(rebate, product, 3m);

        Assert.True(result.Success);
        Assert.Equal(repository.CalculationIdentifier, result.CalculationIdentifier);

        var saved = Assert.Single(repository.SavedCalculations);
        Assert.Same(rebate, saved.Rebate);
        Assert.Equal(25m, saved.Amount);
    }

    [Fact]
    public async Task CalculateAsync_FixedRateRebate_StoresPriceTimesPercentageTimesVolume()
    {
        var repository = new RecordingRebateRepository();
        var service = new RebateService(repository);
        var rebate = new Rebate
        {
            Identifier = Guid.NewGuid(),
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.1m
        };
        var product = new Product
        {
            Price = 20m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };

        var result = await service.CalculateAsync(rebate, product, 3m);

        Assert.True(result.Success);
        Assert.Equal(repository.CalculationIdentifier, result.CalculationIdentifier);
        var saved = Assert.Single(repository.SavedCalculations);
        Assert.Same(rebate, saved.Rebate);
        Assert.Equal(6m, saved.Amount);
    }

    [Fact]
    public async Task CalculateAsync_AmountPerUom_StoresAmountTimesVolume()
    {
        var repository = new RecordingRebateRepository();
        var service = new RebateService(repository);
        var rebate = new Rebate
        {
            Identifier = Guid.NewGuid(),
            Incentive = IncentiveType.AmountPerUom,
            Amount = 2.5m
        };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        var result = await service.CalculateAsync(rebate, product, 4m);

        Assert.True(result.Success);
        Assert.Equal(repository.CalculationIdentifier, result.CalculationIdentifier);
        var saved = Assert.Single(repository.SavedCalculations);
        Assert.Same(rebate, saved.Rebate);
        Assert.Equal(10m, saved.Amount);
    }

    private sealed class RecordingRebateRepository : IRebateRepository
    {
        public Guid CalculationIdentifier { get; } = Guid.NewGuid();
        public List<(Rebate Rebate, decimal Amount)> SavedCalculations { get; } = new();

        public Task<Guid> StoreCalculationResultAsync(Rebate rebate, decimal amount)
        {
            SavedCalculations.Add((rebate, amount));
            return Task.FromResult(CalculationIdentifier);
        }

        public Task<Rebate?> GetByIdentifierAsync(Guid identifier) =>
            throw new NotSupportedException("Calculation tests do not look up rebates.");

        public Task<RebateCalculation?> GetCalculationByIdentifierAsync(Guid identifier) =>
            throw new NotSupportedException("Calculation tests do not look up saved calculations.");
    }
}
