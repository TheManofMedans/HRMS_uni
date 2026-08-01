using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Request;

namespace HRMS.Application.Validators
{
    public class CreateRequestDtoValidator : AbstractValidator<CreateRequestDto>
    {
        public CreateRequestDtoValidator() 
        {
            RuleFor(r => r.EmployeeId).NotNull();
            //RuleFor(r => r.StartDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
            //RuleFor(r => r.EndDate).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(r => r.Type).IsInEnum();
            RuleFor(r => r.Description).MaximumLength(500);
        }
    }
}
