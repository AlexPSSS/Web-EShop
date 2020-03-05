using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;

namespace WebStore.Services.Product
{
    public class InMemoryEmployeesService : IEntityListService<EmployeeViewModel>
    {
        private readonly List<EmployeeViewModel> _employees;

        public InMemoryEmployeesService()
        {
            _employees = new List<EmployeeViewModel>
            {
                new EmployeeViewModel
                {
                    Id = 1,
                    FirstName = "Иван",
                    SurName = "Иванов",
                    Patronymic = "Иванович",
                    Age = 22
                },
                new EmployeeViewModel
                {
                    Id = 2,
                    FirstName = "Владислав",
                    SurName = "Петров",
                    Patronymic = "Иванович",
                    Age = 35
                }
            };


        }

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _employees;
        }

        public EmployeeViewModel GetById(int id)
        {
            return _employees.FirstOrDefault(e => e.Id.Equals(id));
        }

        public void Add(EmployeeViewModel model)
        {
            model.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(model);
        }

        public EmployeeViewModel Edit(int id, EmployeeViewModel Employee)
        {
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            var db_employee = GetById(id);
            if (db_employee is null) return null;

            db_employee.FirstName = Employee.FirstName;
            db_employee.SurName = Employee.SurName;
            db_employee.Patronymic = Employee.Patronymic;
            db_employee.Age = Employee.Age;

            return db_employee;
        }


        public bool Delete(int id)
        {
            var employee = GetById(id);
            return employee != null && _employees.Remove(employee);
        }

        public void SaveChanges()
        {
            // ничего не делаем
        }

    }
}
