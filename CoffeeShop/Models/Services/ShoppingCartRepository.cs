using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CoffeeShopDbContext _context;

        public ShoppingCartRepository(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public Guid ShoppingCartId { get; set; }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            CoffeeShopDbContext context = services.GetService<CoffeeShopDbContext>() ?? throw new Exception("Error al inicializar CoffeeShopDbContext");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCartRepository(context) { ShoppingCartId = Guid.Parse(cartId) };
        }

        public async Task AddToCartAsync(Product product)
        {
            if (product is null)
            {
                return;
            }

            var shoppingCartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(x => x.Product!.Id.Equals(product.Id) && x.ShoppingCartId.Equals(ShoppingCartId));

            if (shoppingCartItem is null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Quantity = 1
                };

                await _context.ShoppingCartItems.AddAsync(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            var cartItems = _context.ShoppingCartItems.Where(x => x.ShoppingCartId.Equals(ShoppingCartId));
            _context.ShoppingCartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
        }

        public decimal GetCartPrice()
        {
            var cartItems = _context.ShoppingCartItems.Where(x => x.ShoppingCartId.Equals(ShoppingCartId));
            var hasProduct = cartItems.Select(x => x.Product).Any();
            decimal totalCost = 0;

            if (hasProduct)
            {
                totalCost = cartItems.Select(x => x.Product!.Price * x.Quantity)
                                     .Sum();

                return totalCost;
            }

            return totalCost;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= await _context.ShoppingCartItems
                                                .Where(x => x.ShoppingCartId.Equals(ShoppingCartId))
                                                .Include(x => x.Product)
                                                .ToListAsync() ?? new List<ShoppingCartItem>();
        }

        public async Task<int> RemoveFromCartAsync(Product product)
        {
            var shoppingCartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(x => x.Product!.Id.Equals(product.Id) && x.ShoppingCartId.Equals(ShoppingCartId));

            var quantity = 0;

            if (shoppingCartItem is not null)
            {
                if (shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                    quantity = shoppingCartItem.Quantity;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            await _context.SaveChangesAsync();

            return quantity;
        }
    }
}
