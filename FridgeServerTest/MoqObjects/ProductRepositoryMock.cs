using Contracts;
using Entities.Models;

namespace FridgeServerTest.MoqObjects;

public class ProductRepositoryMock : IProductRepository
{
    private readonly List<Product> _products;
    
            public ProductRepositoryMock()
            {
                _products = new List<Product>
                {
                    new Product
                    {
                        Id = new Guid("3a51f9d7-df7e-4d19-9673-8f52c7529ea4"),
                        Name = "Apple",
                        DefaultQuantity = 3
                    },
                    new Product
                    {
                        Id = new Guid("9f8bede4-2052-486e-b41e-bd29f64d0bd6"),
                        Name = "Water",
                        DefaultQuantity = 1
                    },
                    new Product
                    {
                        Id = new Guid("9486c981-7af7-4974-9b41-870b2f2c4036"),
                        Name = "Eggs",
                        DefaultQuantity = 10
                    },
                    new Product
                    {
                        Id = new Guid("df022d85-a5fd-40c3-a7dc-47f0a5da6439"),
                        Name = "Milk",
                        DefaultQuantity = 1
                    },
                    new Product
                    {
                        Id = new Guid("f2b3d6aa-db28-421c-a462-5daa37477b24"),
                        Name = "Orange",
                        DefaultQuantity = 2
                    }
                };
            }
    
            public void CreateProduct(Product product)
            {
                product.Id = Guid.NewGuid();
                _products.Add(product);
            }
    
            public void DeleteProduct(Product product)
            {
                _products.Remove(product);
            }
    
            public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges)
            {
                return _products;
            }
    
            public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
            {
                return _products.Where(i => ids.Contains(i.Id));
            }
    
            public async Task<Product> GetProductAsync(Guid productId, bool trackChanges)
            {
                return _products.Where(i => i.Id.Equals(productId)).SingleOrDefault()!;
            }
}