using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebStore.Interfaces.Services;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Models;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEntityListService<EmployeeViewModel>
    {
        public EmployeesClient(IConfiguration config) : base(config, WebAPI.Employees) { }

        public IEnumerable<EmployeeViewModel> GetAll() => Get<List<EmployeeViewModel>>(_ServiceAddress);

        public EmployeeViewModel GetById(int id) => Get<EmployeeViewModel>($"{_ServiceAddress}/{id}");

        public void Add(EmployeeViewModel Employee) => Post(_ServiceAddress, Employee);

        public EmployeeViewModel Edit(int id, EmployeeViewModel Employee)
        {
            var response = Put($"{_ServiceAddress}/{id}", Employee);
            return response.Content.ReadAsAsync<EmployeeViewModel>().Result;
        }

        public bool Delete(int id) => Delete($"{_ServiceAddress}/{id}").IsSuccessStatusCode;

        public void SaveChanges() { }
    }
}
