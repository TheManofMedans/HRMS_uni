using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class MailThread
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? RelatedRequestId { get; set; }
        public Request? RelatedRequest { get; set; }
        public ICollection<MailMessage> Messages { get; set; } = new List<MailMessage>();
        public ICollection<MailThreadParticipant> Participants { get; set; } = new List<MailThreadParticipant>();
    }
}
