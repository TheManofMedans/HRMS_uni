using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.domain.Entities;
using HRMS.Application.DTOs.Shift;

namespace HRMS.Application.Mappings
{
    public class ShiftMappingProfile : Profile
    {
        public ShiftMappingProfile() 
        {
            CreateMap<Shift,ShiftResponseDto>();
            CreateMap<Shift, CompanyDto> ().ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.Id)).
                ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
            CreateMap<CreateShiftDto, Shift>();
        }
    }
}
