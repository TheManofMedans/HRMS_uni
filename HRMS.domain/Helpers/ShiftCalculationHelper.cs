using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Helpers
{
    public static class ShiftCalculationHelper
    {
        public static DateTime CalculateShiftEnd(DateTime date, Shift shift)
        {
            date = date.Date;
            if (shift.StartTime > shift.EndTime)
            {
                date.AddDays(1);
            }
            date.Add(shift.EndTime);
            return date;
        }
        public static DateTime CalculateShiftStart(DateTime date, Shift shift)
        {
            date = date.Date;
            date.Add(shift.StartTime);
            return date;
        }
    }

}
