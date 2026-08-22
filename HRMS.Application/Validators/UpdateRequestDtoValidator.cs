using FluentValidation;
using HRMS.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class UpdateRequestDtoValidator : AbstractValidator<UpdateRequestDto>
    {
        public UpdateRequestDtoValidator() 
        {
            RuleFor(ur => ur.Status).IsInEnum();
            RuleFor(ur => ur.Description).MaximumLength(500);
        }
    }
}
