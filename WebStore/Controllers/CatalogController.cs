using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Interfaces.Services;
using WebStore.Domain.Filters;
using WebStore.Domain.Models;
using WebStore.Infrastructure;
using AutoMapper;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductService productService, IConfiguration Configuration)
        {
            _productService = productService;
            _Configuration = Configuration;
        }

        [SimpleActionFilter]
        public IActionResult Shop(int? SectionId, int? BrandId, [FromServices] IMapper Mapper, int Page = 1)
        {
            var page_size = int.TryParse(_Configuration["PageSize"], out var size) ? size : (int?)null;

            var products = _productService.GetProducts(new ProductFilter
            {
                CategoryId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = page_size
            });

            return View(new CatalogViewModel
            {
                CategoryId = SectionId,
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
            var product = _productService.GetProductById(id);
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
    }
}