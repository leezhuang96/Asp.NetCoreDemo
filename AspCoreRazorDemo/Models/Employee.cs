using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreRazorDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }

        public string FristName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public bool Fired { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
