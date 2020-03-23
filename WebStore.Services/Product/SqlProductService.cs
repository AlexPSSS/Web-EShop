using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.Interfaces.Services;
using WebStore.DAL;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Services.Mapping;

namespace WebStore.Services.Product
{
    public class SqlProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context) => _context = context;

        public IEnumerable<CategoryDTO> GetCategories() => _context.Categories.ToDTO().AsEnumerable();

        public CategoryDTO GetCategoryById(int id) => _context.Categories.Find(id).ToDTO();

        public IEnumerable<BrandDTO> GetBrands() => _context.Brands.ToDTO();

        public BrandDTO GetBrandById(int id) => _context.Brands.Find(id).ToDTO();

        public PagedProductsDTO GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Domain.Entities.Product> query = _context.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.CategoryId != null)
                query = query.Where(product => product.CategoryId == Filter.CategoryId);

            if (Filter?.Ids?.Count > 0)
                query = query.Where(product => Filter.Ids.Contains(product.Id));

            var total_count = query.Count();

            if (Filter?.PageSize != null)
                query = query
                   .Skip((Filter.Page - 1) * (int)Filter.PageSize)
                   .Take((int)Filter.PageSize);

            return new PagedProductsDTO
            {
                Products = query.AsEnumerable().ToDTO(),
                TotalCount = total_count
            };
        }

        /// <summary>
        /// Продукт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Product, если нашел, иначе null</returns>
        public ProductDTO GetProductById(int id)
        {
            return _context.Products
               .Include(p => p.Brand)
               .Include(p => p.Category)
               .FirstOrDefault(p => p.Id == id).ToDTO();
        }

    }
}
