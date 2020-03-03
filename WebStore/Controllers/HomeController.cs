using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _Logger;

        public HomeController(ILogger<HomeController> Logger) => _Logger = Logger;

        //[SimpleActionFilter]
        public IActionResult Index()
        {
            //throw new ApplicationException("Ошибочка вышла...");
            //return Content("Hello from controller");
            //return new EmptyResult();
            //return new FileContentResult();
            //return new NotFoundResult();
            //return new JsonResult("");
            //return PartialView("_Partial/_LeftSideBar");
            //return RedirectToAction("Blog", "Home");
            //return new RedirectResult("https://google.com");
            //return StatusCode(500);
            _Logger.LogInformation("Запрос главной страницы!");
            return View();
        }
        public IActionResult ThrowError(string id) => throw new ApplicationException(id);

        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogSingle()
        {
            return View();
        }
        public IActionResult NotFound404()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}