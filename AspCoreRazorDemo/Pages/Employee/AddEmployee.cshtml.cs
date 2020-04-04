using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCoreRazorDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspCoreRazorDemo.Pages.Employee
{
    public class AddEmployeeModel : PageModel
    {
        private readonly IEmployeeService employeeService;

        [BindProperty]
        public AspCoreRazorDemo.Models.Employee Employee { get; set; }

        public AddEmployeeModel(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> OnPostAsync(int departmentId)
        {
            Employee.DepartmentId = departmentId;

            if (ModelState.IsValid)
            {
                await employeeService.Add(Employee);
                return RedirectToPage("EmployeeList", new { departmentId });
            }
            return Page();
        }
    }
}
