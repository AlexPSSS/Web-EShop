using System.Collections.Generic;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Filters;

namespace WebStore.Interfaces.Services
{
    public interface IProductService
    {
        IEnumerable<SectionDTO> GetCategories();

        SectionDTO GetCategoryById(int id);

        IEnumerable<BrandDTO> GetBrands();

        BrandDTO GetBrandById(int id);

        IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null);

        //PagedProductsDTO GetProducts(ProductFilter Filter = null);

        /// <summary>
        /// Продукт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Product, если нашел, иначе null</returns>
        ProductDTO GetProductById(int id);
    }
}
