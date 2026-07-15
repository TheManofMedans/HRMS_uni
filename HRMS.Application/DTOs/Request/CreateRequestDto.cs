using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Enums;

namespace HRMS.Application.DTOs.Request
{
    public class CreateRequestDto
    {
        public int EmployeeId { get; set; }
        public RequestType RequestType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
