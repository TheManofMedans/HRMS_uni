using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Helpers
{
    public static class PayrollWeekCalculator
    {
        public static readonly TimeSpan RecalculationWindow = TimeSpan.FromDays(30);

        public static DateTime GetWeekStart(DateTime date)
        {
            date = date.Date;
            int daysSinceSaturday = ((int)date.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return date.AddDays(-daysSinceSaturday);
        }
    }
}
