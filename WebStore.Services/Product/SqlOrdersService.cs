using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.Interfaces.Services;
using WebStore.DAL;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Services.Mapping;

namespace WebStore.Services.Product
{
    public class SqlOrdersService : IOrdersService
    {
        private readonly WebStoreContext _context;
        private readonly UserManager<User> _userManager;

        public SqlOrdersService(WebStoreContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _context.Orders
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .Where(order => order.User.UserName == UserName)
            .ToArray()
            .Select(o => o.ToDTO());

        public OrderDTO GetOrderById(int id) =>
            _context.Orders
            .Include("User")
            .Include(order => order.OrderItems)
            .FirstOrDefault(order => order.Id == id)
            .ToDTO();

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = OrderModel.OrderViewModel.Name,
                    Address = OrderModel.OrderViewModel.Address,
                    Phone = OrderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _context.Orders.Add(order);

                foreach (var item in OrderModel.OrderItems)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                    if (product is null)
                        throw new InvalidOperationException($"Товар с идентификатором id:{item.Id} отсутствует в БД");

                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = product
                    };

                    _context.OrderItems.Add(order_item);
                }

                _context.SaveChanges();
                transaction.Commit();

                return order.ToDTO();
            }
        }
    }
}
