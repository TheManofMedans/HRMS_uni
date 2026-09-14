using HRMS.Application.DTOs.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task<MailThreadResponseDto> CreateThreadAsync(CreateMailThreadDto dto);
        Task<MailMessageResponseDto> ReplyAsync(int threadId, SendMailMessageDto dto);
        Task<IEnumerable<MailThreadSummaryDto>> GetMyThreadsAsync();
        Task<MailThreadResponseDto> GetThreadByIdAsync(int threadId);
    }
}
