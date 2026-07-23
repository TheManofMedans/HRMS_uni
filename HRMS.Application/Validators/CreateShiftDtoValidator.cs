using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Shift;

namespace HRMS.Application.Validators
{
    public class CreateShiftDtoValidator : AbstractValidator<CreateShiftDto>
    {
        public CreateShiftDtoValidator() 
        {
            RuleFor(s => s.ShiftName).NotEmpty().MaximumLength(100);
            RuleFor(s => s.CompanyId).NotEmpty();
            RuleFor(s => s.StartTime).NotEmpty();
            RuleFor(s => s.EndTime).NotEmpty();
        }
    }
}
