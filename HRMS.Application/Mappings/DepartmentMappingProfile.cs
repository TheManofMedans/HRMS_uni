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
                ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.DepartmentEmployees)).
                ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
            CreateMap<EmployeeDepartment, EmployeeDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeID)).
                ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName)).
                ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName));
            CreateMap<Company,CompanyDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).
                ForMember(dest => dest.Name,opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.RegNum, opt => opt.MapFrom(src => src.RegNum));
            CreateMap<CreateDepartmentDto, Department>();
        }
    }
}
