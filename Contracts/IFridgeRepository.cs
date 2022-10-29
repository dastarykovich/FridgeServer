using Entities.Models;

namespace Contracts
{
    public interface IFridgeRepository
    {
        Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool trackChanges);

        Task<Fridge> GetFridgeAsync(Guid fridgeId, bool trackChanges);

        void CreateFridge(Fridge fridge);

        Task<IEnumerable<Fridge>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteFridge(Fridge fridge);
    }
}
