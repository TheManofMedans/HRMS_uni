using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Mail
{
    public class MailThreadSummaryDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public DateTime LastMessageAt { get; set; }
        public string LastMessagePreview { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
