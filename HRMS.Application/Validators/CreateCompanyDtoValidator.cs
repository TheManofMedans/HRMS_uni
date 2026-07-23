using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Company;

namespace HRMS.Application.Validators
{
    public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyDtoValidator() 
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Role).NotEmpty();
            RuleFor(c => c.RegNum).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Address).NotEmpty().MaximumLength(500);
            RuleFor(c => c.UserId).NotNull();
        }
    }
}
