using FluentValidation;
using HRMS.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator() 
        {
            RuleFor(uu => uu.FirstName).MaximumLength(100);
            RuleFor(uu => uu.LastName).MaximumLength(100);
            RuleFor(uu => uu.Phone).MaximumLength(11);
            RuleFor(uu => uu.CurrentPass).MaximumLength(100);
            RuleFor(uu => uu.NewPassword).MaximumLength(100);
            RuleFor(uu => uu.Gender).IsInEnum();
        }
    }
}
