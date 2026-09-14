using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Mail
{
    public class MailThreadResponseDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? RelatedRequestId { get; set; }
        public List<int> ParticipantUserIds { get; set; } = new();
        public List<MailMessageResponseDto> Messages { get; set; } = new();
    }
}
