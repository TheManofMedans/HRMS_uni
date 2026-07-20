using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Application.DTOs.Department;
using HRMS.domain.Entities;

namespace HRMS.Application.Mappings
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile() 
        {
            CreateMap<Department,DepartmentResponseDto>().
                ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.DepartmentEmployees));
            CreateMap<EmployeeDepartment, EmployeeDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeID)).
                ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName)).
                ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName));
        }
    }
}
