using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Time
{
    public static class VnDateTimeHelper
    {
        public static bool IsNextDay(this DateTime date, int nextDays)
        {
            if (date.Date >= DateTime.Today.AddDays(nextDays))
                return true;
            return false;
        }

        public static DateTime SetStartWorkingTime(this DateTime date)
        {
            int _year = date.Year;
            int _month = date.Month;
            int _day = date.Day;
            int _hour = 08;
            int _minute = 00;
            int _milisecond = 00;

            DateTime _time = new DateTime(_year, _month, _day, _hour, _minute, _milisecond);
            return _time;
        }
        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }
        public static bool IsInHours(this DateTime date, int min, int max)
        {
            if ((date.TimeOfDay.Hours >= min) && (date.TimeOfDay.Hours <= max)) return true;
            return false;
        }
        public static bool IsPrevDay(this DateTime date, int nextDays)
        {
            if (date.Date <= DateTime.Today.AddDays(-nextDays))
                return true;
            return false;
        }
        public static bool IsExpired(this DateTime date)
        {
            if (date < DateTime.Now)
                return true;
            return false;
        }
        public static bool IsToday(this DateTime date)
        {
            if (date.Date == DateTime.Today)
                return true;
            return false;
        }
        public static bool IsWeekend(this DateTime date)
        {
            if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Saturday))
                return true;
            return false;
        }
        public static bool IsWorkDay(this DateTime date)
        {
            return !date.IsWeekend();
        }
        public static bool IsTomorrow(this DateTime date)
        {
            if (date.Date == DateTime.Today.AddDays(1))
                return true;
            return false;
        }
        public static bool IsYesterday(this DateTime date)
        {
            if (date.Date == DateTime.Today.AddDays(-1))
                return true;
            return false;
        }
        
        public static DateTime AddWorkdays(this DateTime originalDate, int workDays)
        {
            DateTime tmpDate = originalDate;
            while (workDays > 0)
            {
                tmpDate = tmpDate.AddDays(1);
                if (tmpDate.DayOfWeek < DayOfWeek.Saturday &&
                    tmpDate.DayOfWeek > DayOfWeek.Sunday &&
                    !tmpDate.IsHoliday())
                    workDays--;
            }
            return tmpDate;
        }

        //Can add more holidays in Viet Nam Calendar
        public static bool IsHoliday(this DateTime date)
        {
            if (date.IsWeekend()) return true;
            return false;
        }
        public static int GetWorkDays(DateTime start, DateTime end)
        {
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            else if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                end = end.AddDays(-1);
            }
            else if (end.DayOfWeek == DayOfWeek.Sunday)
            {
                end = end.AddDays(-2);
            }

            int diff = (int)end.Subtract(start).TotalDays;

            int result = diff / 7 * 5 + diff % 7;

            if (end.DayOfWeek < start.DayOfWeek)
            {
                return result - 2;
            }
            else
            {
                return result;
            }
        }
    }
}
