using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSore.Interfaces.Services;
using WebStore.Domain.DTO.Orders;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrdersService
    {
        private readonly IOrdersService _OrderService;

        public OrdersApiController(IOrdersService OrderService) => _OrderService = OrderService;

        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _OrderService.GetUserOrders(UserName);

        [HttpGet("{id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int id) => _OrderService.GetOrderById(id);

        [HttpPost("{UserName?}")]
        public OrderDTO CreateOrder([FromBody] CreateOrderModel OrderModel, string UserName) => _OrderService.CreateOrder(OrderModel, UserName);
    }
}