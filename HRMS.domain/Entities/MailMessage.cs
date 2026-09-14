using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class MailMessage
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public MailThread Thread { get; set; } = null!;
        public int SenderUserId { get; set; }
        public User Sender { get; set; } = null!;
        public string Body { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
