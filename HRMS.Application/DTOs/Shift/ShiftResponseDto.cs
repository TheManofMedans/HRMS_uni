using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Shift
{
    public class ShiftResponseDto
    {
        public int Id { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public CompanyDto User { get; set; } = new();
        
    }
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set;} = string.Empty;
    }
}
