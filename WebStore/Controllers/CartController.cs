using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Details()
        {
            var model = new OrderDetailsViewModel
            {
                CartViewModel = _cartService.TransformCart(),
                OrderViewModel = new OrderViewModel()
            };

            return View("Details", model);
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int id, string returnUrl)
        {
            _cartService.AddToCart(id);
            return Redirect(returnUrl);
        }

        /// создание заказа
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel Model, [FromServices] IOrdersService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new OrderDetailsViewModel
                {
                    CartViewModel = _cartService.TransformCart(),
                    OrderViewModel = Model
                });

            var create_order_model = new CreateOrderModel
            {
                OrderViewModel = Model,
                OrderItems = _cartService.TransformCart().Items
                   .Select(item => new OrderItemDTO
                   {
                       Id = item.Key.Id,
                       Price = item.Key.Price,
                       Quantity = item.Value
                   })
                   .ToList()
            };

            var order = OrderService.CreateOrder(create_order_model, User.Identity.Name);

            _cartService.RemoveAll();

            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region API

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddToCartAPI(int id)
        {
            _cartService.AddToCart(id);
            return Json(new { id, message = $"Товар id:{id} был добавлен в корзину" });
        }

        public IActionResult DecrimentFromCartAPI(int id)
        {
            _cartService.DecrementFromCart(id);
            return Json(new { id, message = $"Количество товара с id:{id} в корзине было уменьшено на 1" });
        }

        public IActionResult RemoveFromCartAPI(int id)
        {
            _cartService.RemoveFromCart(id);
            return Json(new { id, message = $"Товар id:{id} был удалён из корзины" });
        }

        public IActionResult RemoveAllAPI()
        {
            _cartService.RemoveAll();
            return Json(new { message = "Корзина была очищена" });
        }

        #endregion
    }
}

