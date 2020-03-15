using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Interfaces.Services;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Filters;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductService
    {
        public ProductsClient(IConfiguration config) : base(config, WebAPI.Products) { }

        public IEnumerable<SectionDTO> GetCategories() => Get<List<SectionDTO>>($"{_ServiceAddress}/categories");

        public SectionDTO GetCategoryById(int id) => Get<SectionDTO>($"{_ServiceAddress}/categories/{id}");

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{_ServiceAddress}/brands/{id}");

        public IEnumerable<BrandDTO> GetBrands() => Get<List<BrandDTO>>($"{_ServiceAddress}/brands");

        public PagedProductsDTO GetProducts(ProductFilter Filter = null) =>
            Post(_ServiceAddress, Filter)
               .Content
               .ReadAsAsync<PagedProductsDTO>()
               .Result;

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");
    }
}
