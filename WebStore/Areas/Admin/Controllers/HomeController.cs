using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSore.Interfaces.Services;
using WebStore.Domain.Filters;

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

        public IActionResult ProductList()
        {
            return View(_productData.GetProducts(new ProductFilter()));
        }
    }
}
