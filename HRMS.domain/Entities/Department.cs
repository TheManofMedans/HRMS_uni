using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public ICollection<EmployeeDepartment> DepartmentEmployees { get; set; } = new List<EmployeeDepartment>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
