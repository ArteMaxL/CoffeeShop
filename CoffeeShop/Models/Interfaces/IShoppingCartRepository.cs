namespace CoffeeShop.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task AddToCartAsync(Product product);
        Task<int> RemoveFromCartAsync(Product product);
        Task<List<ShoppingCartItem>> GetShoppingCartItems();
        Task ClearCartAsync();
        decimal GetCartPrice();
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
    }
}
