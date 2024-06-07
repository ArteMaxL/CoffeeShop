using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;

namespace CoffeeShop.Models.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(CoffeeShopDbContext context, IShoppingCartRepository shoppingCartRepository)
        {
            _context = context;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task PlaceOrderAsync(Order order)
        {
            var shoppingCartItems = await _shoppingCartRepository.GetShoppingCartItemsAsync();
            order.OrderDetails = new List<OrderDetail>();

            foreach (var item in shoppingCartItems)
            {
                if (item.Product is not null)
                {
                    var orderDetail = new OrderDetail
                    {
                        Quantity = item.Quantity,
                        ProductId = item.Product.Id,
                        Price = item.Product.Price
                    };

                    order.OrderDetails.Add(orderDetail);
                }
            }

            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _shoppingCartRepository.GetCartPrice();

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
