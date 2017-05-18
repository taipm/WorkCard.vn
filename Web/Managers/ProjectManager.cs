using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class ProjectManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<WorkIssue> GetIssues(Guid projectId)
        {
            var _issues = db.Issues.Where(t => t.ProjectId == projectId);
            return _issues.AsEnumerable();
        }
    }
}