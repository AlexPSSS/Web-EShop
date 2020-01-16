using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class GoodsController : Controller
    {
        private readonly List<Goods> _goods = new List<Goods>
        {
            new Goods
            {
                Id = 1,
                Description = "Молоток",
                EAN13 = "4602564782511",
                Group = 100,
                Price = 22.34F
            },
            new Goods
            {
                Id = 2,
                Description = "Дрель",
                EAN13 = "4602564782619",
                Group = 202,
                Price = 100500.01F
            }
        };

        // GET: /
        // GET: /goods/
        // GET: /goods/index
        public IActionResult Index()
        {
            return View(_goods);
        }

        // GET: /goods/details/{id}
        public IActionResult Details(int id)
        {
            var goods = _goods.FirstOrDefault(x => x.Id == id);

            //Если такого не существует
            if (goods == null)
                return NotFound(); // возвращаем результат 404 Not Found

            return View(goods);
        }

    }
}