using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Request
{
    public class UpdateRequestDto
    {
        public DateTime? ReviewedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public RequestStatus? Status { get; set; }
    }
}
