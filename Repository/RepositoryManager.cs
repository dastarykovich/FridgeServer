using Contracts;
using Entities.Context;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;

        private IFridgeRepository _fridgeRepository = null!;

        private IFridgeModelRepository _fridgeModelRepository = null!;

        private IFridgeProductRepository _fridgeProductsRepository = null!;

        private IProductRepository _productsRepository = null!;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IFridgeRepository Fridge
        {
            get
            {
                if (_fridgeRepository == null)
                {
                    _fridgeRepository = new FridgeRepository(_repositoryContext);
                }

                return _fridgeRepository;
            }

        }
        public IFridgeModelRepository FridgeModel
        {
            get
            {
                if (_fridgeModelRepository == null)
                {
                    _fridgeModelRepository = new FridgeModelRepository(_repositoryContext);
                }

                return _fridgeModelRepository;
            }
        }
        public IFridgeProductRepository FridgeProduct
        {
            get
            {
                if (_fridgeProductsRepository == null)
                {
                    _fridgeProductsRepository = new FridgeProductRepository(_repositoryContext);
                }

                return _fridgeProductsRepository;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_productsRepository == null)
                {
                    _productsRepository = new ProductRepository(_repositoryContext);
                }

                return _productsRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
