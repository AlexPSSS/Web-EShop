using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebSore.Interfaces.Services;
using WebStore.Domain.Filters;
using WebStore.Domain.Models;
using WebStore.Infrastructure;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;

        public CatalogController(IProductService productService)
        {
            _productService = productService;
        }

        [SimpleActionFilter]
        public IActionResult Shop(int? categoryId, int? brandId, [FromServices] IMapper Mapper)
        {
            // получаем список отфильтрованных продуктов
            var products = _productService.GetProducts(new ProductFilter 
            { 
                BrandId = brandId, 
                CategoryId = categoryId 
            });

            // сконвертируем в CatalogViewModel
            return View( new CatalogViewModel()
            {
                BrandId = brandId,
                CategoryId = categoryId,
                Products = products.Select(Mapper.Map<ProductViewModel>).OrderBy(p => p.Order)
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