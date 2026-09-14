using AutoMapper;
using HRMS.Application.DTOs.Mail;
using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Mappings
{
    public class MailMappingProfile : Profile
    {
        public MailMappingProfile()
        {
            CreateMap<MailMessage, MailMessageResponseDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"));

            CreateMap<MailThread, MailThreadResponseDto>()
                .ForMember(dest => dest.ParticipantUserIds, opt => opt.MapFrom(src => src.Participants.Select(p => p.UserId)));
        }
    }
}
