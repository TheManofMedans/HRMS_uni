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
            CreateMap<Employee, EmployeeResponseDto>().ForMember(dest=> dest.DepartmentNames,
                opt=> opt.MapFrom(src => src.EmployeeDepartments.Select(ed =>ed.Department.Name)));
            CreateMap<CreateEmployeeDto, Employee>().ForMember(dest => dest.EmployeeDepartments, opt => opt.Ignore());
        }
    }
}
