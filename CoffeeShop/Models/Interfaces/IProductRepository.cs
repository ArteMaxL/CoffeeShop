namespace CoffeeShop.Models.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetTrending();
        Product? GetDetail(Guid id);
    }
}
