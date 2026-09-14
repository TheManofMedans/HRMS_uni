using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class MailThreadParticipant
    {
        public int ThreadId { get; set; }
        public MailThread Thread { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime? LastReadAt { get; set; }
    }
}
