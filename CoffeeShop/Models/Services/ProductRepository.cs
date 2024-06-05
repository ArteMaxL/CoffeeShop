using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models.Services
{
    public class ProductRepository : IProductRepository
    {
        private CoffeeShopDbContext _context;

        public ProductRepository(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product?>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetDetailAsync(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<IEnumerable<Product?>> GetTrendingAsync()
        {
            return await _context.Products.Where(p => p.IsTrendingProduct).ToListAsync();
        }
    }
}
