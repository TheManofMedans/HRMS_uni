using FluentValidation;
using HRMS.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterDto>
    {
        public RegisterUserValidator() 
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(r => r.LastName).NotEmpty().MaximumLength(100);
            RuleFor(r => r.PhoneNumber).NotEmpty();
            RuleFor(r => r.Password).NotEmpty().MinimumLength(8);
            RuleFor(r => r.SSN).NotEmpty().MinimumLength(10);
        }
    }
}
