using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Helpers
{
    public static class IssuesHelper
    {
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