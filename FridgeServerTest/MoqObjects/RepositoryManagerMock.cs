using Contracts;

namespace FridgeServerTest.MoqObjects
{
    public class RepositoryManagerMock : IRepositoryManager
    {
        private IFridgeRepository? _fridgeRepository;

        private IFridgeModelRepository? _fridgeModelRepository;

        private IFridgeProductRepository? _fridgeProductRepository;

        private IProductRepository? _productRepository;

        public IFridgeRepository Fridge
        {
            get
            {
                if (_fridgeRepository == null)
                {
                    _fridgeRepository = new FridgeRepositoryMock();
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
                    _fridgeModelRepository = new FridgeModelRepositoryMock();
                }

                return _fridgeModelRepository;
            }
        }
        public IFridgeProductRepository FridgeProduct
        {
            get
            {
                if (_fridgeProductRepository == null)
                {
                    _fridgeProductRepository = new FridgeProductRepositoryMock();
                }

                return _fridgeProductRepository;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepositoryMock();
                }

                return _productRepository;
            }
        }

        public async Task SaveAsync()
        {
            return;
        }
    }
}
