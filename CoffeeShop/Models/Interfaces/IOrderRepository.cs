namespace CoffeeShop.Models.Interfaces
{
    public interface IOrderRepository
    {
        Task PlaceOrderAsync(Order order);
    }
}
