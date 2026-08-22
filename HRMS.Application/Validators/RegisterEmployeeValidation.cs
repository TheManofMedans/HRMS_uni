using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Employee;

namespace HRMS.Application.Validators
{
    public class RegisterEmployeeValidation : AbstractValidator<RegisterEmployeeDto>
    {
        public RegisterEmployeeValidation() 
        {
            RuleFor(er => er.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(er => er.Address).MaximumLength(500);
            RuleFor(er => er.FirstName).MaximumLength(100).NotEmpty();
            RuleFor(er => er.LastName).MaximumLength(100).NotEmpty();
            RuleFor(er => er.SSN).NotEmpty().MinimumLength(10).MaximumLength(10);
            RuleFor(er => er.Phone).NotEmpty();
            RuleFor(er => er.Password).NotEmpty().MinimumLength(8);
            RuleFor(er => er.HireDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(er => er.AssignedAt).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(er => er.EmploymentStatus).IsInEnum();
            RuleFor(er => er.departmentId).NotEmpty();
            RuleFor(er => er.PayrollStatus).IsInEnum();
           // RuleFor(er => er.JobDescription).MaximumLength(500);
        }
    }
}
