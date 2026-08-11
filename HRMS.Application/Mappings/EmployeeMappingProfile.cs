using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Application.DTOs.Employee;
using HRMS.domain.Entities;

namespace HRMS.Application.Mappings
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile() 
        { 
            CreateMap<Employee, EmployeeResponseDto>().ForMember(dest=> dest.Departments,
                opt=> opt.MapFrom(src => src.EmployeeDepartments));
            CreateMap<EmployeeDepartment, DepartmentDto>()
                .ForMember(dto => dto.Name,opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.DepartmentID))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(src => src.Department.Description));
            CreateMap<CreateEmployeeDto, Employee>().ForMember(dest => dest.EmployeeDepartments, opt => opt.Ignore());
            CreateMap<RegisterEmployeeDto, Employee>();
        }
    }
}
