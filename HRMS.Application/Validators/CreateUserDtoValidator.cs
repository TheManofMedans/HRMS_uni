using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.User;

namespace HRMS.Application.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator() 
        {
            RuleFor(u => u.Email).NotEmpty().MaximumLength(256);
            RuleFor(u => u.Role).NotEmpty();
            RuleFor(u => u.Phone).NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.SSN).NotEmpty().MaximumLength(10);
        }
    }
}
