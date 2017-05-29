using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using CafeT.Text;

namespace Web.Helpers
{
    public static class IssuesHelper
    {
        #region IssueMining
        public static IEnumerable<WorkIssue> HasEmail(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.Content.HasEmail() || t.Description.HasEmail()).ToList();
        }
        #endregion
        #region HelperForFilter
        public static IEnumerable<WorkIssue> GetNoTime(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.IsNoTime()).ToList();
        }
        public static IEnumerable<WorkIssue> GetDone(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.Status == IssueStatus.Done).ToList();
        }
        public static IEnumerable<WorkIssue> GetExpired(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.IsExpired()).ToList();
        }
        public static IEnumerable<WorkIssue> GetNotDone(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.Status != IssueStatus.Done).ToList();
        }
        public static IEnumerable<WorkIssue> GetFromNow(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.End.HasValue && t.End.Value >= DateTime.Now).ToList();
        }
        public static IEnumerable<WorkIssue> GetToday(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.IsToday());
        }
        public static IEnumerable<WorkIssue> GetInDay(this IEnumerable<WorkIssue> issues, DateTime date)
        {
            return issues.Where(t => t.IsInDay(date));
        }
        //public static IEnumerable<WorkIssue> GetInDays(this IEnumerable<WorkIssue> issues, DateTime start, DateTime end)
        //{
        //    List<WorkIssue> _results = new List<WorkIssue>();
        //    //for(var year = start.Year; year <= end.Year; year++)
        //    //{
        //    //    for(var month = start.Month; month <= end.Month; month ++)
        //    //    {
        //    //        for(var day = start.da)
        //    //    }
        //    //}

        //}
        public static IEnumerable<WorkIssue> GetTomorrow(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.IsTomorrow()).ToList();
        }
        public static IEnumerable<WorkIssue> GetNext(this IEnumerable<WorkIssue> issues, int nextDay)
        {
            return issues.Where(t => t.IsNextDay(nextDay)).ToList();
        }
        public static IEnumerable<WorkIssue> GetPrev(this IEnumerable<WorkIssue> issues, int nextDay)
        {
            return issues.Where(t => t.IsPrevDay(nextDay)).ToList();
        }
        #endregion
        public static double TotalMinutesTimeTodo(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.TimeToDo.HasValue).Sum(t => t.TimeToDo.Value);
        }
        public static double TotalHoursTimeTodo(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.TimeToDo.HasValue).Sum(t => t.TimeToDo.Value)/60;
        }

        public static double TotalDaysTimeTodo(this IEnumerable<WorkIssue> issues)
        {
            return issues.Where(t => t.TimeToDo.HasValue).Sum(t => t.TimeToDo.Value) / (60*8);
        }
    }
}