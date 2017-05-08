using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.ModelViews
{
    
    public class WorkView
    {
        public List<WorkIssue> Issues { set; get; }

        public List<WorkIssue> GetToday()
        {
            return Issues.Where(t => t.IsToday()).ToList();
        }

        public List<WorkIssue> GetTomorrow()
        {
            return Issues.Where(t => t.IsToday()).ToList();
        }
        public List<WorkIssue> GetYesterday()
        {
            return Issues.Where(t => t.IsToday()).ToList();
        }

        public List<WorkIssue> GetCreatedBy(string userName)
        {
            return Issues.Where(t => t.CreatedBy.ToLower() == userName.ToLower()).ToList();
        }

        public List<WorkIssue> GetUpdatedBy(string userName)
        {
            return Issues.Where(t => t.UpdatedBy.ToLower() == userName.ToLower()).ToList();
        }
    }
}