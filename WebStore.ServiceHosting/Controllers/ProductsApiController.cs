using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSore.Interfaces.Services;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductService
    {
        private readonly IProductService _ProductData;

        public ProductsApiController(IProductService ProductData) => _ProductData = ProductData;

        [HttpGet("sections")]
        public IEnumerable<Category> GetSections() => _ProductData.GetCategories();


        [HttpGet("sections")]
        public IEnumerable<Category> GetCategories() => _ProductData.GetCategories();


        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _ProductData.GetBrands();

        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter Filter = null) => _ProductData.GetProducts(Filter);

        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id) => _ProductData.GetProductById(id);
    }
}