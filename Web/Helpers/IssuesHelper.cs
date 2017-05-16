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
        public static List<WorkIssue> HasEmail(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.Content.HasEmail() || t.Description.HasEmail()).ToList();
        }
        //public static WorkIssue Next(this WorkIssue issue, List<WorkIssue> issues)
        //{
        //    if (!issues.Contains(issue)) return null;
        //    else
        //    {
        //        issues.OrderByDescending(t => t.End);
        //        _next = issues.GetNext()
        //    }
        //}
        #endregion
        #region HelperForFilter
        public static List<WorkIssue> GetNoTime(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.IsNoTime()).ToList();
        }
        public static List<WorkIssue> GetDone(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.Status == IssueStatus.Done).ToList();
        }
        public static List<WorkIssue> GetExpired(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.IsExpired()).ToList();
        }
        public static List<WorkIssue> GetNotDone(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.Status != IssueStatus.Done).ToList();
        }
        public static List<WorkIssue> GetToday(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.IsToday()).ToList();
        }
        public static List<WorkIssue> GetTomorrow(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.IsTomorrow()).ToList();
        }
        public static List<WorkIssue> GetNext(this List<WorkIssue> issues, int nextDay)
        {
            return issues.Where(t => t.IsNextDay(nextDay)).ToList();
        }
        public static List<WorkIssue> GetPrev(this List<WorkIssue> issues, int nextDay)
        {
            return issues.Where(t => t.IsPrevDay(nextDay)).ToList();
        }
        #endregion
        public static double TotalMinutesTimeTodo(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.TimeToDo.HasValue).Sum(t => t.TimeToDo.Value);
        }
        public static double TotalHoursTimeTodo(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.TimeToDo.HasValue).Sum(t => t.TimeToDo.Value)/60;
        }

        public static double TotalDaysTimeTodo(this List<WorkIssue> issues)
        {
            return issues.Where(t => t.TimeToDo.HasValue).Sum(t => t.TimeToDo.Value) / (60*8);
        }
    }
}