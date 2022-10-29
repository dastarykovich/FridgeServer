using Entities.Models;

namespace Contracts
{
    public interface IFridgeProductRepository
    {
        Task<IEnumerable<FridgeProduct>> GetFridgeProductsAsync(bool trackChanges);

        Task<FridgeProduct> GetFridgeProductAsync(Guid fridgeProductId, bool trackChanges);

        void CreateFridgeProduct(Guid fridgeId, Guid productId, FridgeProduct fridgeProduct);

        void DeleteFridgeProduct(FridgeProduct fridgeProduct);
    }
}
