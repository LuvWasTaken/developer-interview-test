using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data.Repositories;

public class ProductRepository(RebateDbContext context) : IProductRepository
{
    public Task<Product?> GetByIdentifierAsync(Guid identifier)
    {
        return context.Products.AsNoTracking().FirstOrDefaultAsync(product => product.Identifier == identifier);
    }
}
