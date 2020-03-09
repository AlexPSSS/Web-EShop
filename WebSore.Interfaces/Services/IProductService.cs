using System.Collections.Generic;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;

namespace WebStore.Interfaces.Services
{
    public interface IProductService
    {
        IEnumerable<Category> GetCategories();
        SectionDTO GetCategoryById(int id);

        IEnumerable<Brand> GetBrands();
        BrandDTO GetBrandById(int id);

        IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null);

        /// <summary>
        /// Продукт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Product, если нашел, иначе null</returns>
        ProductDTO GetProductById(int id);
    }
}
