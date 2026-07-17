using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Company
{
    public class CreateCompanyDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RegNum { get; set; } = string.Empty;
        public string Address {get; set;} = string.Empty;
        public CompanyRole Role {  get; set;}

    }
}
