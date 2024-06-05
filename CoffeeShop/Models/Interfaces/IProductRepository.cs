namespace CoffeeShop.Models.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product?>> GetAllAsync();
        Task<IEnumerable<Product?>> GetTrendingAsync();
        Task<Product?> GetDetailAsync(Guid id);
    }
}
