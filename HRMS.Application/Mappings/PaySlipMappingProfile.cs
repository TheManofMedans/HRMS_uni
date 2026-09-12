using AutoMapper;
using HRMS.Application.DTOs.PaySlip;
using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Mappings
{
    public class PaySlipMappingProfile : Profile
    {
        public PaySlipMappingProfile() 
        {
            CreateMap<PaySlip, PaySlipResponseDto>().ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department));
            CreateMap<Department, DepartmentDto>();
        }

    }
}
