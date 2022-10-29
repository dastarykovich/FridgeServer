using Contracts;
using Entities.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Product> GetProductAsync(Guid productId, bool trackChanges) =>
            (await FindByCondition(p => p.Equals(productId), trackChanges)
                .SingleOrDefaultAsync())!;

        public void CreateProduct(Product product) =>
            Create(product);

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(p => ids.Contains(p.Id), trackChanges).ToListAsync();

        public void DeleteProduct(Product product) =>
            Delete(product);
    }
}
