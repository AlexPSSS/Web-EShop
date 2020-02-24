using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSore.Interfaces.Services;
using WebStore.Domain.Models;

namespace WebStore.Controllers
{
    // ~/employee
    [Route("users")]
    // ~/users
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEntityListService<EmployeeViewModel> _entityListService;

        public EmployeeController(IEntityListService<EmployeeViewModel> entityListService)
        {
            _entityListService = entityListService;
        }

        // GET: /
        // GET: /home/
        // GET: /home/index
        [Route("all")]
        // ~/users/all
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_entityListService.GetAll());
            //return Content("Hello from controller");
        }

        // GET: /home/details/{id}
        [Route("{id}")]
        // ~/users/1111
        public IActionResult Details(int id)
        {
            var employee = _entityListService.GetById(id);

            //Если такого не существует
            if (employee == null)
                return NotFound(); // возвращаем результат 404 Not Found

            return View(employee);
        }

        /// <summary>
        /// Добавление или редактирование сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("edit/{id?}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return View(new EmployeeViewModel());

            EmployeeViewModel model = _entityListService.GetById(id.Value);
            if (model == null)
                return NotFound();// возвращаем результат 404 Not Found

            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.Age < 18 || model.Age > 100)
            {
                ModelState.AddModelError("Age", "Ошибка возраста!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Id > 0) // если есть Id, то редактируем модель
            {
                var dbItem = _entityListService.GetById(model.Id);

                if (object.ReferenceEquals(dbItem, null))
                    return NotFound();// возвращаем результат 404 Not Found

                dbItem.FirstName = model.FirstName;
                dbItem.SurName = model.SurName;
                dbItem.Age = model.Age;
                dbItem.Patronymic = model.Patronymic;
                dbItem.Position = model.Position;
            }
            else // иначе добавляем модель в список
            {
                _entityListService.Add(model);
            }
            _entityListService.SaveChanges(); // станет актуальным позднее (когда добавим БД)

            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _entityListService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}