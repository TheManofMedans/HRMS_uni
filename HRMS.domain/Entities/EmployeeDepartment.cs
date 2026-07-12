using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class EmployeeDepartment
    {
        public int EmployeeID {  get; set; }
        public Employee Employee { get; set; } = null!;
        public int DepartmentID { get; set; }
        public Department Department { get; set; } = null!;
        public Boolean IsPrimary { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
