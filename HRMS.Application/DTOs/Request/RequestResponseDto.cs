using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Request
{
    public class RequestResponseDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public RequestStatus Status { get; set; }
        public RequestType RequestType { get; set; }
        public EmployeeDto Employee { get; set; }
        

    }
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
