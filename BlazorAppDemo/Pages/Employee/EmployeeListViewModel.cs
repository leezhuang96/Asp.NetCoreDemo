using BlazorAppDemo.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppDemo.Pages.Employee
{
    public class EmployeeListViewModel: ComponentBase
    {
        [Parameter]
        public string DepartmentId { get; set; }
        public IEnumerable<BlazorAppDemo.Models.Employee> Employees { get; set; }
        [Inject]
        protected IEmployeeService EmployeeService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employees = await EmployeeService.GetByDepartmentId(int.Parse(DepartmentId));
        }
    }
}
