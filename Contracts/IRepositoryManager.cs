namespace Contracts
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge { get; }

        IFridgeModelRepository FridgeModel { get; }

        IFridgeProductRepository FridgeProduct { get; }

        IProductRepository Product { get; }

        Task SaveAsync();
    }
}
