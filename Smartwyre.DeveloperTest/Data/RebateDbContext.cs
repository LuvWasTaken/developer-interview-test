using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDbContext(DbContextOptions<RebateDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Rebate> Rebates => Set<Rebate>();
    public DbSet<RebateCalculation> RebateCalculations => Set<RebateCalculation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rebate>().HasKey(rebate => rebate.Identifier);
        modelBuilder.Entity<Product>().HasKey(product => product.Id);
        modelBuilder.Entity<RebateCalculation>().HasKey(calculation => calculation.Id);
    }
}
