using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using HRMS.Application.DTOs.User;
using HRMS.domain.Entities;

namespace HRMS.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User,UserResponseDto>()
                .ForMember(dest => dest.Companies, opt => opt.MapFrom(src => src.UserCompanies));
            CreateMap<UserCompany, UserCompanyDto>().ForMember(dest => dest.CompanyId, opt => opt.MapFrom(opt => opt.CompanyId))
                .ForMember(dest => dest.CompanyName,opt => opt.MapFrom(src => src.company.Name))
                .ForMember(dest => dest.Role,opt => opt.MapFrom(src => src.Role));
            CreateMap<CreateUserDto, User>().
                ForMember(dest => dest.PhoneNumber,opt => opt.MapFrom(src => src.Phone));

        }
    }
}
