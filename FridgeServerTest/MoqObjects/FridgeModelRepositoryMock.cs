using Contracts;
using Entities.Models;
using Entities.RequestFeatures;

namespace FridgeServerTest.MoqObjects
{
    public class FridgeModelRepositoryMock : IFridgeModelRepository
    {
        private readonly List<FridgeModel> _fridgeModels;

        public FridgeModelRepositoryMock()
        {
            _fridgeModels = new List<FridgeModel>
            {
                new FridgeModel
                {
                    Id = new Guid("aaf10bfc-4b57-4794-a509-87fe5cda85c1"),
                    FridgeId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    Name = "GA-B379SLUL",
                    Year = 2019
                },
                new FridgeModel
                {
                    Id = new Guid("f3da5705-d9c9-402d-9742-cc60d508bcf3"),
                    FridgeId = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                    Name = "RB37A5200WW/WT",
                    Year = 2020
                },
                new FridgeModel
                {
                    Id = new Guid("27edb4dd-8458-47cf-9ea4-d990353124e4"),
                    FridgeId = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                    Name = "KGV39XL2AR",
                    Year = 2022
                },
            };
        }

        public void CreateFridgeModel(Guid fridgeId, FridgeModel fridgeModel)
        {
            fridgeModel.FridgeId = fridgeId;
            _fridgeModels.Add(fridgeModel);
        }

        public void DeleteFridgeModel(FridgeModel fridgeModel)
        {
            _fridgeModels.Remove(fridgeModel);
        }

        public async Task<FridgeModel> GetFridgeModelAsync(Guid fridgeId, Guid fridgeModelId,
            bool trackChanges)
        {
            return _fridgeModels.FirstOrDefault(i => i.Id.Equals(fridgeModelId))!;
        }

        public async Task<PagedList<FridgeModel>> GetFridgeModelsAsync(Guid fridgeId,
            FridgeModelParameters fridgeModelParameters, bool trackChanges)
        {
            var fridgeModels = _fridgeModels.Where(i => i.FridgeId.Equals(fridgeId)).ToList();

            return new PagedList<FridgeModel>(fridgeModels, fridgeModels.Count(),
                fridgeModelParameters.PageNumber, fridgeModelParameters.PageSize);
        }
    }
}
