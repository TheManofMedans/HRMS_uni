using FluentValidation;
using HRMS.Application.DTOs.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class UpdateShiftDtoValidator : AbstractValidator<UpdateShiftDto>
    {
        public UpdateShiftDtoValidator() 
        {
            RuleFor(us => us.ShiftName).MaximumLength(100);
        }
    }
}
