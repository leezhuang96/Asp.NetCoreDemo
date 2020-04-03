using AspCoreDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreDemo.ViewComponents
{
    public class CompanySummaryViewComponent : ViewComponent
    {
        private readonly IDepartmentService departmentService;

        public CompanySummaryViewComponent(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string title)
        {
            ViewBag.Title = title;
            var companySummary = await departmentService.GetCompanySummary();
            return View(companySummary);
        }
    }
}
