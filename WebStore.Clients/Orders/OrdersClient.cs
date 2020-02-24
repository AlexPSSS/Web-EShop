using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebSore.Interfaces.Services;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrdersService
    {
        public OrdersClient(IConfiguration config) : base(config, WebAPI.Orders) { }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => Get<List<OrderDTO>>($"{_ServiceAddress}/user/{UserName}");

        public OrderDTO GetOrderById(int id) => Get<OrderDTO>($"{_ServiceAddress}/{id}");

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName) =>
            Post($"{_ServiceAddress}/{UserName}", OrderModel)
               .Content
               .ReadAsAsync<OrderDTO>()
               .Result;
    }
}
