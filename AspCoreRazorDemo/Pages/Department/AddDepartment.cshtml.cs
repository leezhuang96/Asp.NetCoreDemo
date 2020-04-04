using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCoreRazorDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspCoreRazorDemo.Pages.Department
{
    public class AddDepartmentModel : PageModel
    {
        private readonly IDepartmentService departmentService;

        [BindProperty]
        public AspCoreRazorDemo.Models.Department Department { get; set; }

        public AddDepartmentModel(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await departmentService.Add(Department);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
