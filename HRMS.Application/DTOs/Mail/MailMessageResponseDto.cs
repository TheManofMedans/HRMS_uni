using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Mail
{
    public class MailMessageResponseDto
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
