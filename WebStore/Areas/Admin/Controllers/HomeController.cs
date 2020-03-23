using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.Filters;
using AutoMapper;
using System.Linq;
using WebStore.Domain.Models;
using WebStore.Domain.Entities;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IProductService _productData;

        public HomeController(IProductService productData)
        {
            _productData = productData;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList([FromServices] IMapper Mapper)
        {
            var productListDTO = _productData.GetProducts(new ProductFilter()).Products;
            var productList = productListDTO.Select(Mapper.Map<Product>).OrderBy(p => p.Order);
            return View(productList);
        }
    }
}
