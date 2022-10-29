using Contracts;
using Entities.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();
        public async Task<Fridge> GetFridgeAsync(Guid fridgeId, bool trackChanges) =>
            (await FindByCondition(i => i.Id.Equals(fridgeId), trackChanges)
                .SingleOrDefaultAsync())!;

        public void CreateFridge(Fridge fridge) => Create(fridge);

        public async Task<IEnumerable<Fridge>> GetByIdsAsync(IEnumerable<Guid> ids,
            bool trackChanges) =>
                await FindByCondition(i => ids.Contains(i.Id), trackChanges).ToListAsync();

        public void DeleteFridge(Fridge fridge) => Delete(fridge);
    }
}
