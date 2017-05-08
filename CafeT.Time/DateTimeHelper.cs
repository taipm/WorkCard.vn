using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Time
{
    public static class VnDateTimeHelper
    {
        public static bool IsToday(this DateTime date)
        {
            if (date.Date == DateTime.Today)
                return true;
            return false;
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
    }

    public static class DateTimeHelper
    {
        public static DateTime? LaterNearest(this List<DateTime> list)
        {
            list.SortAscending();
            foreach(var item in list)
            {
                if (DateTime.Compare(item, DateTime.Now) > 0) return item;
            }
            return null;
        }

        public static List<DateTime> SortAscending(this List<DateTime> list)
        {
            list.Sort((a, b) => a.CompareTo(b));
            return list;
        }

        public static List<DateTime> SortDescending(this List<DateTime> list)
        {
            list.Sort((a, b) => b.CompareTo(a));
            return list;
        }

        public static List<DateTime> SortMonthAscending(this List<DateTime> list)
        {
            list.Sort((a, b) => a.Month.CompareTo(b.Month));
            return list;
        }

        public static List<DateTime> SortMonthDescending(this List<DateTime> list)
        {
            list.Sort((a, b) => b.Month.CompareTo(a.Month));
            return list;
        }
    }
}
