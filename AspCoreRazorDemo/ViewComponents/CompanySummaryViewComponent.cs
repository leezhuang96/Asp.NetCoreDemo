using AspCoreRazorDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspCoreRazorDemo.ViewComponents
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
