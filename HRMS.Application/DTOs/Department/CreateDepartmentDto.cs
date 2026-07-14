using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        public string name {  get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public int CompanyId { get; set; }

    }
}
