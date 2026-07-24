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
            CreateMap<Attendance, AttendanceResponseDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<Shift, ShiftDto>();
            CreateMap<CreateAttendanceDto, Attendance>();
        }
    }
}
