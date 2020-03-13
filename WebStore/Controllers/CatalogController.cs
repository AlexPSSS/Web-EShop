using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Interfaces.Services;
using WebStore.Domain.Filters;
using WebStore.Domain.Models;
using AutoMapper;
using WebStore.Domain.DTO.Products;
using System.Collections.Generic;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __PageSize = "PageSize";

        private readonly IProductService _ProductService;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductService ProductData, IConfiguration Configuration)
        {
            _ProductService = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Shop(int? CategoryId, int? BrandId, [FromServices] IMapper Mapper, int Page = 1)
        {
            var page_size = int.TryParse(_Configuration[__PageSize], out var size) ? size : (int?)null;

            var products = _ProductService.GetProducts(new ProductFilter
            {
                CategoryId = CategoryId,
                BrandId = BrandId,
                Page = Page,
                PageSize = page_size
            });

            return View(new CatalogViewModel
            {
                CategoryId = CategoryId,
                BrandId = BrandId,
                Products = products.Products.Select(Mapper.Map<ProductViewModel>).OrderBy(p => p.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = page_size ?? 0,
                    PageNumber = Page,
                    TotalItems = products.TotalCount
                }
            });
        }

        [Route("{id}")]
        public IActionResult ProductDetails(int id)
        {
            var product = _ProductService.GetProductById(id);
            if (product == null)
                return NotFound();
            return (View(new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                Brand = product.Brand != null ? product.Brand.Name : string.Empty
            })
                );
        }

        #region API

        public IActionResult GetFiltratedItems(int? CategoryId, int? BrandId, [FromServices] IMapper Mapper, int Page)
        {
            var products = GetProducts(CategoryId, BrandId, Page);
            return PartialView("_Partial/_ProductItems", products.Select(Mapper.Map<ProductViewModel>));
        }

        private IEnumerable<ProductDTO> GetProducts(int? categoryId, int? BrandId, int Page) =>
            _ProductService.GetProducts(new ProductFilter
            {
                CategoryId = categoryId,
                BrandId = BrandId,
                Page = Page,
                PageSize = int.Parse(_Configuration[__PageSize])
            }).Products;

        #endregion
    }

}