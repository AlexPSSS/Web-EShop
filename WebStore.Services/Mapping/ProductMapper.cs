using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this WebStore.Domain.Entities.Product p) => p is null ? null : new ProductDTO
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
            Section = p.Category is null ? null : new SectionDTO
            {
                Id = p.Category.Id,
                Name = p.Category.Name
            }
        };

        public static WebStore.Domain.Entities.Product FromDTO(this ProductDTO p) => p is null ? null : new WebStore.Domain.Entities.Product
        {
            Id = p.Id
        };

    }
}
