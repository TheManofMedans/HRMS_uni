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
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.CompanyNames, opt => opt.MapFrom(src => src.UserCompanies.Select(d => d.company.Name)));

        }
    }
}
