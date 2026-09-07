using AutoMapper;
using HRMS.Application.DTOs.Notification;
using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Mappings
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile() 
        {
            CreateMap<Notification, NotificationResponseDto>()
                .ForMember(dest => dest.User,opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Request, opt => opt.MapFrom(src => src.Request))
                .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Attendance.Shift))
                .ForMember(dest => dest.Attendance, opt => opt.MapFrom(src => src.Attendance));
            CreateMap<Shift, ShiftDto>();
            CreateMap<Attendance, AttendanceDto>().
                ForMember(dest => dest.DepartmentName,opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<Request,RequestDto>();
            CreateMap<User, UserDto>();
        }
    }
}
