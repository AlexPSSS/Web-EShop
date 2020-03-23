using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this Domain.Entities.Product p) => p is null ? null : new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Order = p.Order,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            Brand = p.Brand is null ? null : new BrandDTO
            {
                Id = p.Brand.Id,
                Name = p.Brand.Name
            },
            Category = p.Category is null ? null : new CategoryDTO
            {
                Id = p.Category.Id,
                Name = p.Category.Name
            }
        };

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Domain.Entities.Product> Products) => Products?.Select(ToDTO);

        //public static Domain.Entities.Product FromDTO(this ProductDTO p) => p is null ? null : new WebStore.Domain.Entities.Product
        //{
        //    Id = p.Id
        //};
    }
}
