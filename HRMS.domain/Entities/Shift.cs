using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Attendance > Attendances { get; set; } = new List<Attendance>();
    }
}
