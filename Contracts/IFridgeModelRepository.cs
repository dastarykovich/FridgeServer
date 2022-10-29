using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IFridgeModelRepository
    {
        Task<PagedList<FridgeModel>> GetFridgeModelsAsync(Guid fridgeId,
            FridgeModelParameters fridgeModelParameters, bool trackChanges);

        Task<FridgeModel> GetFridgeModelAsync(Guid fridgeId, Guid fridgeModelId,
            bool trackChanges);

        void CreateFridgeModel(Guid fridgeId, FridgeModel fridgeModel);

        void DeleteFridgeModel(FridgeModel fridgeModel);
    }
}
