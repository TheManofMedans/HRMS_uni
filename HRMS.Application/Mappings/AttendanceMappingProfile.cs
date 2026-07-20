using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using AutoMapper;
using HRMS.Application.DTOs.Attendance;

namespace HRMS.Application.Mappings
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile() 
        {
            CreateMap<Attendance, AttendanceResponseDto>().
                ForMember(dest => dest.department.id, opt => opt.MapFrom(src => src.department.Id)).
                ForMember(dest => dest.department.Name, opt => opt.MapFrom(src => src.department.Name)).
                ForMember(dest => dest.employee.id,opt => opt.MapFrom(src => src.Employee.Id)).
                ForMember(dest => dest.employee.FirstName,opt => opt.MapFrom(src => src.Employee.FirstName)).
                ForMember(dest => dest.employee.LastName,opt => opt.MapFrom(src => src.Employee.LastName)).
                ForMember(dest => dest.employee.Email,opt => opt.MapFrom(src => src.Employee.Email)).
                ForMember(dest => dest.employee.Phone, opt => opt.MapFrom(src => src.Employee.Phone)).
                ForMember(dest => dest.shift.id,opt => opt.MapFrom(src => src.shift.Id)).
                ForMember(dest => dest.shift.ShiftName,opt => opt.MapFrom(src => src.shift.ShiftName)).
                ForMember(dest => dest.shift.StartTime,opt => opt.MapFrom(src => src.shift.StartTime)).
                ForMember(dest => dest.shift.EndTime,opt => opt.MapFrom(src => src.shift.EndTime));
            
        }
    }
}
