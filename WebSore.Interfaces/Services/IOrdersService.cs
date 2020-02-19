using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebSore.Interfaces.Services
{
    public interface IOrdersService
    {
        IEnumerable<Order> GetUserOrders(string userName);
        Order GetOrderById(int id);
        Order CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }
}