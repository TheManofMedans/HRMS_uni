using FluentValidation;
using HRMS.Application.DTOs.Employee;
using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator() 
        {
            RuleFor(ue => ue.FirstName).MaximumLength(100);
            RuleFor(ue => ue.LastName).MaximumLength(100);
            RuleFor(ue => ue.Phone).MaximumLength(11);
            RuleFor(ue => ue.Salary).MaximumLength(11);
            RuleFor(ue => ue.EmploymentStatus).IsInEnum();
            RuleFor(ue => ue.PayrollStatus).IsInEnum();
            RuleFor(ue => ue.Gender).IsInEnum();
            RuleFor(ue => ue.JobDescription).MinimumLength(500);
        }
    }
}
