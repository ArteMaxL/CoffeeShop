namespace CoffeeShop.Models
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public Guid ShoppingCartId { get; set; }
    }
}
