using Contracts;
using Entities.Models;

namespace FridgeServerTest.MoqObjects
{
    public class FridgeRepositoryMock : IFridgeRepository
    {
        private readonly List<Fridge> _fridges;

        public FridgeRepositoryMock()
        {
            _fridges = new List<Fridge>
            {
                new Fridge
                {
                    Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    Name = "LG",
                    OwnerName = "Mike"
                },
                new Fridge
                {
                    Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                    Name = "Samsung",
                    OwnerName = "Alex"
                },
                new Fridge
                {
                    Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                    Name = "Bosch",
                    OwnerName = "John"
                }
            };
        }

        public void CreateFridge(Fridge fridge)
        {
            fridge.Id = Guid.NewGuid();
            _fridges.Add(fridge);
        }

        public void DeleteFridge(Fridge fridge)
        {
            _fridges.Remove(fridge);
        }

        public async Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool trackChanges)
        {
            return _fridges;
        }

        public async Task<IEnumerable<Fridge>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return _fridges.Where(i => ids.Contains(i.Id));
        }

        public async Task<Fridge> GetFridgeAsync(Guid fridgeId, bool trackChanges)
        {
            return _fridges.Where(i => i.Id.Equals(fridgeId)).SingleOrDefault()!;
        }
    }
}
