using System;
using System.Collections.Generic;
using System.Linq;
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
            
        }
    }
}
