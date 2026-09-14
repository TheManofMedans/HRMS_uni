using AutoMapper;
using HRMS.Application.DTOs.Mail;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services
{
    public class MailService : IMailService
    {
        private readonly IMapper _mapper;
        private readonly IMailRepository _mailRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        public MailService(IMapper mapper, IMailRepository mailRepository, IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _mapper = mapper;
            _mailRepository = mailRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }
        public async Task<MailThreadResponseDto> GetThreadByIdAsync(int threadId)
        {
            if (_currentUser.UserId is null)
                throw new ForbiddenException("No authenticated user found.");
            int userId = _currentUser.UserId.Value;

            var thread = await _mailRepository.GetByIdAsync(threadId);
            if (thread is null)
                throw new NotFoundException(nameof(MailThread), threadId);

            var participant = thread.Participants.FirstOrDefault(p => p.UserId == userId);
            if (participant is null)
                throw new ForbiddenException("You are not a participant in this thread.");

            if (!participant.IsRead)
            {
                participant.IsRead = true;
                participant.LastReadAt = DateTime.UtcNow;
                await _mailRepository.SaveChangesAsync();
            }

            return _mapper.Map<MailThreadResponseDto>(thread);
        }
        public async Task<IEnumerable<MailThreadSummaryDto>> GetMyThreadsAsync()
        {
            if (_currentUser.UserId is null)
                throw new ForbiddenException("No authenticated user found.");
            int userId = _currentUser.UserId.Value;

            var threads = await _mailRepository.GetForUserAsync(userId);

            return threads.Select(t =>
            {
                var lastMessage = t.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
                var participant = t.Participants.First(p => p.UserId == userId);

                return new MailThreadSummaryDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    LastMessageAt = lastMessage?.SentAt ?? t.CreatedAt,
                    LastMessagePreview = lastMessage is null
                        ? string.Empty
                        : lastMessage.Body.Length > 100 ? lastMessage.Body[..100] + "..." : lastMessage.Body,
                    IsRead = participant.IsRead
                };
            }).OrderByDescending(t => t.LastMessageAt).ToList();
        }
        public async Task<MailThreadResponseDto> CreateThreadAsync(CreateMailThreadDto dto)
        {
            if (_currentUser.UserId is null)
                throw new ForbiddenException("No authenticated user found.");
            int senderId = _currentUser.UserId.Value;

            if (dto.RecipientUserIds is null || dto.RecipientUserIds.Count == 0)
                throw new ConflictException("At least one recipient is required.");

            var senderCompanies = await _userRepository.GetAllCompanyIdsForUserAsync(senderId);
            if (!senderCompanies.Contains(dto.CompanyId))
                throw new ForbiddenException("You are not associated with the specified company.");

            foreach (var recipientId in dto.RecipientUserIds.Distinct())
            {
                if (recipientId == senderId) continue;

                var recipient = await _userRepository.GetByIdAsync(recipientId);
                if (recipient is null)
                    throw new NotFoundException(nameof(User), recipientId);

                var recipientCompanies = await _userRepository.GetAllCompanyIdsForUserAsync(recipientId);
                if (!recipientCompanies.Contains(dto.CompanyId))
                    throw new ConflictException($"User {recipientId} is not associated with the specified company.");
            }

            var thread = new MailThread
            {
                Subject = dto.Subject,
                CompanyId = dto.CompanyId,
                CreatedByUserId = senderId,
                RelatedRequestId = dto.RelatedRequestId
            };

            thread.Messages.Add(new MailMessage
            {
                SenderUserId = senderId,
                Body = dto.InitialMessage
            });

            thread.Participants.Add(new MailThreadParticipant
            {
                UserId = senderId,
                IsRead = true,
                LastReadAt = DateTime.UtcNow
            });

            foreach (var recipientId in dto.RecipientUserIds.Distinct().Where(id => id != senderId))
            {
                thread.Participants.Add(new MailThreadParticipant { UserId = recipientId, IsRead = false });
            }

            await _mailRepository.AddThreadAsync(thread);
            var saved = await _mailRepository.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to create the mail thread.");

            return _mapper.Map<MailThreadResponseDto>(thread);
        }
        public async Task<MailMessageResponseDto> ReplyAsync(int threadId, SendMailMessageDto dto)
        {
            if (_currentUser.UserId is null)
                throw new ForbiddenException("No authenticated user found.");
            int userId = _currentUser.UserId.Value;

            var thread = await _mailRepository.GetByIdAsync(threadId);
            if (thread is null)
                throw new NotFoundException(nameof(MailThread), threadId);

            var participant = thread.Participants.FirstOrDefault(p => p.UserId == userId);
            if (participant is null)
                throw new ForbiddenException("You are not a participant in this thread.");

            var message = new MailMessage
            {
                ThreadId = threadId,
                SenderUserId = userId,
                Body = dto.Body
            };
            thread.Messages.Add(message);

            foreach (var p in thread.Participants)
            {
                p.IsRead = p.UserId == userId;
                if (p.UserId == userId)
                    p.LastReadAt = DateTime.UtcNow;
            }

            var saved = await _mailRepository.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to send the reply.");

            return _mapper.Map<MailMessageResponseDto>(message);
        }
    }
}
