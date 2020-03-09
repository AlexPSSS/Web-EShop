using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;

namespace WebStore.ServiceHosting.Controllers
{
    public class EmployeesApiController : ControllerBase, IEntityListService<EmployeeViewModel>
    {
        private readonly IEntityListService<EmployeeViewModel> _EmployeesData;

        public EmployeesApiController(IEntityListService<EmployeeViewModel> EmployeesData) => _EmployeesData = EmployeesData;

        [HttpGet, ActionName("Get")]
        public IEnumerable<EmployeeViewModel> GetAll() => _EmployeesData.GetAll();

        [HttpGet("{id}"), ActionName("Get")]
        public EmployeeViewModel GetById(int id) => _EmployeesData.GetById(id);

        [HttpPost, ActionName("Post")]
        public void Add([FromBody] EmployeeViewModel Employee) => _EmployeesData.Add(Employee);

        [HttpPut("{id}"), ActionName("Put")]
        public EmployeeViewModel Edit(int id, [FromBody] EmployeeViewModel Employee) => _EmployeesData.Edit(id, Employee);

        [HttpDelete("{id}")]
        public bool Delete(int id) => _EmployeesData.Delete(id);

        [NonAction]
        public void SaveChanges() => _EmployeesData.SaveChanges();
    }

}