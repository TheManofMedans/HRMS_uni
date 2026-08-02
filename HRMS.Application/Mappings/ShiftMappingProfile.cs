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
            CreateMap<Company, CompanyDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateShiftDto, Shift>();
        }
    }
}
