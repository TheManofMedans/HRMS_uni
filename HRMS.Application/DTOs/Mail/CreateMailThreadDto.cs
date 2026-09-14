using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.DTOs.Mail
{
    public class CreateMailThreadDto
    {
        public string Subject { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public List<int> RecipientUserIds { get; set; } = new();
        public string InitialMessage { get; set; } = string.Empty;
        public int? RelatedRequestId { get; set; }
    }
}
