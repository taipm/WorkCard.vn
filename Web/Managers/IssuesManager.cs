using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class IssuesManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Question> GetQuestion(WorkIssue issue)
        {
            return db.Questions.Where(t => t.IssueId.HasValue && t.IssueId == issue.Id).AsEnumerable();
        }

        public IEnumerable<string> GetLinks(WorkIssue issue)
        {
            List<string> _links = new List<string>();
            _links.AddRange(issue.Links);
            var _questions = GetQuestion(issue);
            foreach(var _question in _questions)
            {
                _links.AddRange(_question.Links);
            }
            return _links.Distinct().AsEnumerable();
        }
    }
}