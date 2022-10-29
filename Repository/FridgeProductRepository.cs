using Contracts;
using Entities.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<FridgeProduct>> GetFridgeProductsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<FridgeProduct> GetFridgeProductAsync(Guid fridgeProductId,
            bool trackChanges) => (await FindByCondition(i => 
                i.Id.Equals(fridgeProductId), trackChanges).SingleOrDefaultAsync())!;

        public void CreateFridgeProduct(Guid fridgeId, Guid productId, FridgeProduct fridgeProduct)
        {
            fridgeProduct.FridgeId = fridgeId;
            fridgeProduct.ProductId = productId;
            Create(fridgeProduct);
        }

        public void DeleteFridgeProduct(FridgeProduct fridgeProduct) => Delete(fridgeProduct);
    }
}
