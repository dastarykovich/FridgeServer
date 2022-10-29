using Contracts;
using Entities.Models;

namespace FridgeServerTest.MoqObjects;

public class FridgeProductRepositoryMock : IFridgeProductRepository
{
    private readonly List<FridgeProduct> _fridgeProducts;

        public FridgeProductRepositoryMock()
        {
            _fridgeProducts = new List<FridgeProduct>
            {
                new FridgeProduct
                {
                    Id = new Guid("ee5267cb-1594-4d65-916e-fd2bbff00a3d"),
                    ProductId = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4"),
                    FridgeId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    Quantity = 2
                },
                new FridgeProduct
                {
                    Id = new Guid("9cbb6093-6c64-415f-8470-439177d50350"),
                    ProductId = new Guid("9f8bede4-2052-486e-b41e-bd29f64d0bd6"),
                    FridgeId = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                    Quantity = 3
                },
                new FridgeProduct
                {
                    Id = new Guid("cb7f9b7f-4a88-471e-b07c-82becdcd1842"),
                    ProductId = new Guid("9486c981-7af7-4974-9b41-870b2f2c4036"),
                    FridgeId = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                    Quantity = 1
                },
                new FridgeProduct
                {
                    Id = new Guid("3b1dcb9c-cf1e-40ef-9e89-24a08e186797"),
                    ProductId = new Guid("df022d85-a5fd-40c3-a7dc-47f0a5da6439"),
                    FridgeId = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                    Quantity = 2
                },
                new FridgeProduct
                {
                    Id = new Guid("f8aebf5f-85e0-433b-b143-49d7e1a4745a"),
                    ProductId = new Guid("f2b3d6aa-db28-421c-a462-5daa37477b24"),
                    FridgeId = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                    Quantity = 0
                }
            };
        }

        public void CreateFridgeProduct(Guid fridgeId, Guid productId, FridgeProduct fridgeProduct)
        {
            fridgeProduct.FridgeId = fridgeId;
            fridgeProduct.ProductId = productId;
            _fridgeProducts.Add(fridgeProduct);
        }

        public void DeleteFridgeProduct(FridgeProduct fridgeProduct)
        {
            _fridgeProducts.Remove(fridgeProduct);
        }

        public async Task<FridgeProduct> GetFridgeProductAsync(Guid fridgeProductId, bool trackChanges)
        {
            return _fridgeProducts.FirstOrDefault(i => i.Id.Equals(fridgeProductId))!;
        }

        public async Task<IEnumerable<FridgeProduct>> GetFridgeProductsAsync(bool trackChanges) =>
             _fridgeProducts!;
        
        public async Task<FridgeProduct> FindByParameters(Guid fridgeId, Guid productId, int quantity) =>
            _fridgeProducts.SingleOrDefault(fp => fp!.FridgeId.Equals(fridgeId) &&
                                                  fp.ProductId.Equals(productId) &&
                                                  fp.Quantity.Equals(quantity))!;
}