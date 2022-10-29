using Contracts;
using Entities.Context;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository
{
    public class FridgeModelRepository : RepositoryBase<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<FridgeModel>> GetFridgeModelsAsync(Guid fridgeId,
            FridgeModelParameters fridgeModelParameters, bool trackChanges)
        {
            var fridgeModels = FindByCondition(i => i.FridgeId.Equals(fridgeId),
                trackChanges)
                .FilterFridgeModels(fridgeModelParameters.MinYear, fridgeModelParameters.MaxYear)
                .Search(fridgeModelParameters.SearchTerm)
                .Sort(fridgeModelParameters.OrderBy)
                .Skip((fridgeModelParameters.PageNumber - 1) * fridgeModelParameters.PageSize)
                .Take(fridgeModelParameters.PageSize)
                .ToList();

            var count = await FindByCondition(i => i.FridgeId.Equals(fridgeId), trackChanges)
                .CountAsync();

            return new PagedList<FridgeModel>(fridgeModels, count,
                fridgeModelParameters.PageNumber, fridgeModelParameters.PageSize,
                fridgeModelParameters.SearchTerm);
        }

        public async Task<FridgeModel> GetFridgeModelAsync(Guid fridgeId, Guid fridgeModelId,
            bool trackChanges) =>
                (await FindByCondition(i => i.Id.Equals(fridgeModelId), trackChanges)
                    .SingleOrDefaultAsync())!;

        public void CreateFridgeModel(Guid fridgeId, FridgeModel fridgeModel)
        {
            fridgeModel.FridgeId = fridgeId;
            Create(fridgeModel);
        }

        public void DeleteFridgeModel(FridgeModel fridgeModel) => Delete(fridgeModel);
    }
}
