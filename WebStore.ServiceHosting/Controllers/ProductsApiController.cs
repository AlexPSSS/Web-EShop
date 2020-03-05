using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>Контроллер каталога товаров</summary>
    [Route(WebAPI.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductService
    {
        private readonly IProductService _ProductData;

        public ProductsApiController(IProductService ProductData) => _ProductData = ProductData;

        /// <summary>Получение всех разделов каталога товаров</summary>
        /// <returns>Перечисление всех разделов каталога</returns>
        [HttpGet("categories")]
        public IEnumerable<Category> GetCategories() => _ProductData.GetCategories();

        /// <summary>Получение всех брендов товаров из каталога</summary>
        /// <returns>Перечисление брендов товаров каталога</returns>
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _ProductData.GetBrands();

        /// <summary>Получение товаров, удовлетворяющих критерию поиска</summary>
        /// <param name="Filter">Фильтр - критерий поиска товаров в каталоге</param>
        /// <returns>Перечисление всех товаров из каталога, удовлетворяющих критерию поиска</returns>
        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter Filter = null) => _ProductData.GetProducts(Filter);

        /// <summary>Получение информации по товару, заданному идентификатором</summary>
        /// <param name="id">Идентификатор товара, информацию по которому требуется получить</param>
        /// <returns>Информацию по товару, заданному идентификатором</returns>
        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id) => _ProductData.GetProductById(id);
    }
}