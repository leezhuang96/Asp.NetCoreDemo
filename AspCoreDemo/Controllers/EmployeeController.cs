using AspCoreDemo.Models;
using AspCoreDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreDemo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
        }

        public async Task<IActionResult> Index(int departmentId)
        {
            var department = await departmentService.GetById(departmentId);

            ViewBag.Title = $"Employees of {department.Name}";
            ViewBag.DepartmentId = departmentId;

            var employees = await employeeService.GetByDepartmentId(departmentId);
            return View(employees);
        }

        public IActionResult Add(int departmentId)
        {
            ViewBag.Title = "Add Employee";
            return View(new Employee 
            {
                DepartmentId = departmentId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await employeeService.Add(employee);
            }
            return RedirectToAction(nameof(Index), new { departmentId = employee.DepartmentId});
        }


        public async Task<IActionResult> Frie(int empolyeeId)
        {
            var employee = await employeeService.Frie(empolyeeId);
            return RedirectToAction(nameof(Index), new { departmentId = employee.DepartmentId});
        }
    }
}
