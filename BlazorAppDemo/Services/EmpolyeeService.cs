using BlazorAppDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppDemo.Services
{
    public class EmpolyeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public EmpolyeeService()
        {
            _employees.Add(new Employee
            {
                Id = 1,
                DepartmentId = 1,
                FristName = "Jniffer",
                LastName = "Huang",
                Gender = Gender.Female
            });
            _employees.Add(new Employee
            {
                Id = 2,
                DepartmentId = 1,
                FristName = "Nina",
                LastName = "Wang",
                Gender = Gender.Female
            });
            _employees.Add(new Employee
            {
                Id = 3,
                DepartmentId = 2,
                FristName = "Charles",
                LastName = "Jia",
                Gender = Gender.Male
            });
            _employees.Add(new Employee
            {
                Id = 4,
                DepartmentId = 2,
                FristName = "Vicky",
                LastName = "Wu",
                Gender = Gender.Female
            });
            _employees.Add(new Employee
            {
                Id = 5,
                DepartmentId = 3,
                FristName = "Yolanda",
                LastName = "Huang",
                Gender = Gender.Female
            });
            _employees.Add(new Employee
            {
                Id = 6,
                DepartmentId = 3,
                FristName = "Monica",
                LastName = "Huang",
                Gender = Gender.Female
            });
        }

        public Task Add(Employee employee)
        {
            employee.Id = _employees.Max(x => x.Id) + 1;
            _employees.Add(employee);
            return Task.CompletedTask;
        }

        public Task<Employee> Frie(int id)
        {
            return Task.Run(() => 
            {
                var employee = _employees.FirstOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    employee.Fired = true;
                    return employee;
                }
                return employee;
            });
        }

        public Task<IEnumerable<Employee>> GetByDepartmentId(int departmentId)
        {
            return Task.Run(() => _employees.Where(x => x.DepartmentId == departmentId));
        }

        public Task<Employee> GetById(int id)
        {
            return Task.Run(() => _employees.FirstOrDefault(x => x.Id == id));
        }
    }
}
