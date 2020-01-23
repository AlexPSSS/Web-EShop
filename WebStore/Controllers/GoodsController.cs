using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class GoodsController : Controller
    {
        private readonly IGoodsService _goodsService;

        public GoodsController(IGoodsService goodsService)
        {
            _goodsService = goodsService;
        }

        // GET: /
        // GET: /goods/
        // GET: /goods/index
        public IActionResult Index()
        {
            return View(_goodsService.GetAll());
        }

        // GET: /goods/details/{id}
        public IActionResult Details(int id)
        {
            var goods = _goodsService.GetById(id);

            //Если такого не существует
            if (goods == null)
                return NotFound(); // возвращаем результат 404 Not Found

            return View(goods);
        }
        /// <summary>
        /// Добавление или редактирование товара
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return View(new GoodsView());

            GoodsView model = _goodsService.GetById(id.Value);
            if (model == null)
                return NotFound();// возвращаем результат 404 Not Found

            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]

        public IActionResult Edit(GoodsView model)
        {
            if (model.Id > 0) // если есть Id, то редактируем модель
            {
                var dbItem = _goodsService.GetById(model.Id);

                if (ReferenceEquals(dbItem, null))
                    return NotFound();// возвращаем результат 404 Not Found

                dbItem.Description = model.Description;
                dbItem.EAN13 = model.EAN13;
                dbItem.Group = model.Group;
                dbItem.Price = model.Price;
            }
            else // иначе добавляем модель в список
            {
                _goodsService.AddNew(model);
            }
            _goodsService.Commit(); // станет актуальным позднее (когда добавим БД)

            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _goodsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}