using System.Collections.Generic;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface IOrdersService
    {
        OrderDTO GetOrderById(int id);
        //OrderDTO CreateOrder(OrderViewModel orderModel, CartViewModel cartModel, string userName);
        OrderDTO CreateOrder(CreateOrderModel orderModel, string userName);
        IEnumerable<OrderDTO> GetUserOrders(string userName);
    }
}