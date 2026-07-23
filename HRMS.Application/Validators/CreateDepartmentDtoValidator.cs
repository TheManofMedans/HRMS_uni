using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Department;

namespace HRMS.Application.Validators
{
    public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentDtoValidator() 
        {
            RuleFor(d => d.name).NotEmpty().MaximumLength(100);
            RuleFor(d => d.description).MaximumLength(300);
            RuleFor(d => d.CompanyId).NotEmpty();
        }
    }
}
