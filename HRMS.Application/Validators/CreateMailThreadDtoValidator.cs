using FluentValidation;
using HRMS.Application.DTOs.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Validators
{
    public class CreateMailThreadDtoValidator :AbstractValidator<CreateMailThreadDto>
    {
        public CreateMailThreadDtoValidator()
        {
            RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
            RuleFor(x => x.InitialMessage).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.RecipientUserIds).NotEmpty();
        }
    }
}
