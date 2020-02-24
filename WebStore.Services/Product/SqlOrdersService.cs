using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSore.Interfaces.Services;
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

        public OrderDTO CreateOrder(OrderViewModel orderModel, string userName)
        {
            var user = _userManager.FindByNameAsync(userName).Result;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var order = new Order()
                {
                    Address = orderModel.Address,
                    Name = orderModel.Name,
                    Date = DateTime.Now,
                    Phone = orderModel.Phone,
                    User = user
                };

                _context.Orders.Add(order);

                foreach (var item in orderModel.Items)
                {
                    var productVm = item.Key;
                    var product = _context.Products.FirstOrDefault(p => p.Id.Equals(productVm.Id));
                    if (product == null)
                        throw new InvalidOperationException("Продукт не найден в базе");

                    var orderItem = new OrderItem()
                    {
                        Price = product.Price,
                        Quantity = item.Value,

                        Order = order,
                        Product = product
                    };

                    _context.OrderItems.Add(orderItem);
                }

                _context.SaveChanges();
                transaction.Commit();

                return order.ToDTO();
            }
        }
    }
}
