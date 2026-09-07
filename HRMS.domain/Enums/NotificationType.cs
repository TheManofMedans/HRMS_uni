using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Enums
{
    public enum NotificationType
    {
        RequestSubmitted,
        RequestApproved,
        RequestDenied,
        RequestUpdated,
        AttendanceCreated,
        AttendanceClockIn,
        AttendanceClockOut,
        AttendanceInfoUpdate,
        AttendanceUpdate
    }
}
