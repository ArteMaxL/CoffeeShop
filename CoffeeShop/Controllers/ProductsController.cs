using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Shop()
        {
            return View(await _productRepository.GetAllAsync());
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            return View(await _productRepository.GetDetailAsync(id));
        }

    }
}
