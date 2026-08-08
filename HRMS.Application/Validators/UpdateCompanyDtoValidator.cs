using FluentValidation;
using HRMS.Application.DTOs.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyDtoValidator() 
        {
            RuleFor(c => c.Address).MaximumLength(500);
        }
    }
}
