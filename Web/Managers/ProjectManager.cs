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
        public IEnumerable<Question> GetQuestions(Guid projectId)
        {
            var _questions = db.Questions.Where(t => t.ProjectId == projectId);
            return _questions.AsEnumerable();
        }
        public IEnumerable<Comment> GetComments(Guid projectId)
        {
            var _comments = db.Comments.Where(t => t.ProjectId == projectId);
            return _comments.AsEnumerable();
        }
        public IEnumerable<Contact> GetContacts(Guid projectId)
        {
            var _contacts = db.Contacts.Where(t => t.ProjectId == projectId);
            return _contacts.AsEnumerable();
        }
    }
}