using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Application.DTOs.Company;
using HRMS.domain.Entities;

namespace HRMS.Application.Mappings
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile() 
        {
            CreateMap<Company,CompanyResponseDto>().
                ForMember(dest => dest.Users,opt => opt.MapFrom(src => src.UserCompanies));
            CreateMap<UserCompany,UserDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId)).
                ForMember(dest => dest.FirstName,opt => opt.MapFrom(src => src.user.FirstName)).
                ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.user.LastName)).
                ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email));
            CreateMap<CreateCompanyDto, Company>();
            
        }
    }
}
