using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _shoppingCartRepository.GetShoppingCartItemsAsync();
            _shoppingCartRepository.ShoppingCartItems = items;
            ViewBag.CartTotal = _shoppingCartRepository.GetCartPrice();
            return View(items);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(Guid id)
        {
            var product = await _productRepository.GetDetailAsync(id);

            if (product is not null)
            {
                await _shoppingCartRepository.AddToCartAsync(product);
                var cartItems = await _shoppingCartRepository.GetShoppingCartItemsAsync();
                int cartCount = 0;

                if (cartItems is not null)
                {
                    cartCount = cartItems.Sum(x => x.Quantity);
                }

                HttpContext.Session.SetInt32("CartCount", cartCount);
            }

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> RemoveFromShoppingCart(Guid id)
        {
            var product = await _productRepository.GetDetailAsync(id);

            if (product is not null)
            {
                await _shoppingCartRepository.RemoveFromCartAsync(product);
                var cartItems = await _shoppingCartRepository.GetShoppingCartItemsAsync();
                int cartCount = 0;

                if (cartItems is not null)
                {
                    cartCount = cartItems.Sum(x => x.Quantity);
                }

                HttpContext.Session.SetInt32("CartCount", cartCount);
            }

            return RedirectToAction("Index");
        }
    }
}
