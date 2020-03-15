using System.Collections.Generic;

namespace WebStore.Domain.DTO.Products
{
    public class PagedProductsDTO
    {
        public IEnumerable<ProductDTO> Products { get; set; }

        public int TotalCount { get; set; }
    }
}
