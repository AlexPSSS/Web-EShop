using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Interfaces.Services;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductService
    {
        public ProductsClient(IConfiguration config) : base(config, WebAPI.Products) { }

        public IEnumerable<Category> GetCategories() => Get<List<Category>>($"{_ServiceAddress}/categories");

        public IEnumerable<Brand> GetBrands() => Get<List<Brand>>($"{_ServiceAddress}/brands");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) =>
            Post(_ServiceAddress, Filter)
               .Content
               .ReadAsAsync<List<ProductDTO>>()
               .Result;

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");
    }
}
