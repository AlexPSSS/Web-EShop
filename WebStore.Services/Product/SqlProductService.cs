using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebSore.Interfaces.Services;
using WebStore.DAL;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;

namespace WebStore.Services.Product
{
    public class SqlProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }

        public IEnumerable<Domain.Entities.Product> GetProducts(ProductFilter filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();
            if (filter.BrandId.HasValue)
                query = query.Where(c => c.BrandId.HasValue && c.BrandId.Value.Equals(filter.BrandId.Value));
            if (filter.CategoryId.HasValue)
                query = query.Where(c => c.CategoryId.Equals(filter.CategoryId.Value));

            return query.ToList();
        }

        /// <summary>
        /// Продукт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Product, если нашел, иначе null</returns>
        public Domain.Entities.Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id.Equals(id));
        }

    }
}
