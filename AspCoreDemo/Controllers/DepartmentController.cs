using AspCoreDemo.Models;
using AspCoreDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreDemo.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IOptions<AspCoreDemoOptions> options;

        public DepartmentController(IDepartmentService departmentService, IOptions<AspCoreDemoOptions> options)
        {
            this.departmentService = departmentService;
            this.options = options;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Department Index";
            var departments = await departmentService.GetAll();
            return View(departments);
        }

        public IActionResult Add()
        {
            ViewBag.Title = "Add Department";
            return View(new Department());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentService.Add(department);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
