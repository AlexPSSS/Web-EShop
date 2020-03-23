using System.Collections.Generic;
using WebStore.Domain.DTO.Orders;

namespace WebStore.Interfaces.Services
{
    public interface IOrdersService
    {
        OrderDTO GetOrderById(int id);
        OrderDTO CreateOrder(CreateOrderModel orderModel, string userName);
        IEnumerable<OrderDTO> GetUserOrders(string userName);
    }
}