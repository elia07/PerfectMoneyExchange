using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Utils
{
    public static class DateUtils
    {
        public static int GetDayofWeek(DayOfWeek dow)
        {
            switch(dow)
            {
                case DayOfWeek.Saturday:
                    return 0;
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                default:
                    return -1;
            }
        }
        public static DateTime GetNextWeekEnd(string calenderType)
        {
            int offsetDay = 0;
            if(calenderType.ToLower()=="persian")
            {
                offsetDay = Math.Abs((GetDayofWeek(DayOfWeek.Friday)) - (GetDayofWeek(DateTime.Now.DayOfWeek)));
            }
            else
            {
                offsetDay = Math.Abs((GetDayofWeek(DayOfWeek.Saturday)) - (GetDayofWeek(DateTime.Now.DayOfWeek)));
            }
            return DateTime.Now.Date.AddDays(offsetDay);
        }

        public static DateTime GetNextMonthEnd(string calenderType)
        {
            int offsetDay = 0;
            if (calenderType.ToLower() == "persian")
            {
                PersianCalendar ps = new PersianCalendar();
                offsetDay = Math.Abs(ps.GetDayOfMonth(DateTime.Now) - ps.GetDaysInMonth(ps.GetYear(DateTime.Now), ps.GetMonth(DateTime.Now)));
            }
            else
            {
                offsetDay = Math.Abs(DateTime.Now.Date.Day- DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            return DateTime.Now.Date.AddDays(offsetDay);
        }
    }
}
