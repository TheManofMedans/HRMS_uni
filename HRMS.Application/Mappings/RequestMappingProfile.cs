using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Application.DTOs.Request;
using HRMS.domain.Entities;

namespace HRMS.Application.Mappings
{
    public class RequestMappingProfile : Profile
    {
        public RequestMappingProfile() 
        {
            CreateMap<Request, EmployeeDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Employee.Id)).
                ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName)).
                ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName));
        }
    }
}
