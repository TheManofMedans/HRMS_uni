using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Attendance;

namespace HRMS.Application.Validators
{
    public class CreateAttendanceDtoValidator : AbstractValidator<CreateAttendanceDto>
    {
        public CreateAttendanceDtoValidator() 
        {
            RuleFor(a => a.AttendanceStatus).IsInEnum();
            RuleFor(a =>a.Date).NotEmpty();
            RuleFor(a => a.EmployeeId).NotEmpty();
            RuleFor(a => a.ShiftId).NotEmpty();
            RuleFor(a => a.DepartmentId).NotEmpty();
        }
    }
}
