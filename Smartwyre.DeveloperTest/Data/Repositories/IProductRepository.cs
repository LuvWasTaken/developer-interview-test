using System.Threading.Tasks;
using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdentifierAsync(Guid identifier);
}
