using Bogus;
using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner.Data;

// Adds some sample data to the database for development purposes. This is not intended for production use.
public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(RebateDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        if (await context.Products.AnyAsync(cancellationToken) ||
            await context.Rebates.AnyAsync(cancellationToken) ||
            await context.RebateCalculations.AnyAsync(cancellationToken))
        {
            return;
        }

        var products = new Faker<Product>()
            .UseSeed(12345)
            .RuleFor(product => product.Identifier, faker => faker.Random.Guid())
            .RuleFor(product => product.Price, faker => decimal.Round(faker.Random.Decimal(1m, 100m), 2))
            .RuleFor(product => product.Uom, faker => faker.PickRandom("kg", "L", "each"))
            .RuleFor(product => product.SupportedIncentives, _ =>
                SupportedIncentiveType.FixedRateRebate |
                SupportedIncentiveType.AmountPerUom |
                SupportedIncentiveType.FixedCashAmount)
            .Generate(10);

        var rebateFaker = new Faker<Rebate>()
            .UseSeed(54321)
            .RuleFor(rebate => rebate.Identifier, faker => faker.Random.Guid())
            .RuleFor(rebate => rebate.Amount, faker => decimal.Round(faker.Random.Decimal(1m, 25m), 2))
            .RuleFor(rebate => rebate.Percentage, faker => decimal.Round(faker.Random.Decimal(0.01m, 0.25m), 2));

        var rebates = Enum.GetValues<IncentiveType>().Select(incentive =>
        {
            var rebate = rebateFaker.Generate();
            rebate.Incentive = incentive;
            if (incentive == IncentiveType.FixedRateRebate)
            {
                rebate.Amount = 0m;
            }
            else
            {
                rebate.Percentage = 0m;
            }
            return rebate;
        }).ToList();

        context.Products.AddRange(products);
        context.Rebates.AddRange(rebates);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {ProductCount} products and {RebateCount} rebates. Sample product: {ProductIdentifier}",
            products.Count, rebates.Count, products[0].Identifier);
        foreach (var rebate in rebates)
        {
            logger.LogInformation("Sample {Incentive} rebate: {RebateIdentifier}", rebate.Incentive, rebate.Identifier);
        }
    }
}
