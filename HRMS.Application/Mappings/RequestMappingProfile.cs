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
            CreateMap<Request, RequestResponseDto>()
                .ForMember(dest => dest.Employee,opt => opt.MapFrom(src => src.Employee));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateRequestDto, Request>();
        }
    }
}
