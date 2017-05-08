using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class IssuesManager
    {
        public IEnumerable<WorkIssue> GetIssuesOnToday() { return null; }
        public IEnumerable<WorkIssue> GetIssuesOnTomorrow() { return null; }
        public IEnumerable<WorkIssue> GetIssuesOnYesterday() { return null; }
        public IEnumerable<WorkIssue> GetIssuesOnNextWeek() { return null; }
        public IEnumerable<WorkIssue> GetIssuesOnThisWeek() { return null; }
        public IEnumerable<WorkIssue> GetIssuesOnLastWeek() { return null; }
    }
}