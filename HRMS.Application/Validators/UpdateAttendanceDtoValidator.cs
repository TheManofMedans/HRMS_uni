using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HRMS.Application.DTOs.Attendance;
using HRMS.domain.Enums;

namespace HRMS.Application.Validators
{
    public class UpdateAttendanceDtoValidator : AbstractValidator<UpdateAttendanceDto>
    {
        public UpdateAttendanceDtoValidator() 
        {
        }
    }
}
