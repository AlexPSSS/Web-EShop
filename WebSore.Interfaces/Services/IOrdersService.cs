using System.Collections.Generic;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebSore.Interfaces.Services
{
    public interface IOrdersService
    {
        IEnumerable<OrderDTO> GetUserOrders(string userName);
        OrderDTO GetOrderById(int id);
        OrderDTO CreateOrder(OrderViewModel orderModel, string userName);
    }
}