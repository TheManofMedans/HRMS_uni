using FluentValidation;
using HRMS.Application.DTOs.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator() 
        {
            RuleFor(d => d.Name).MaximumLength(100);
            RuleFor(d => d.Description).MaximumLength(500);
        }
    }
}
