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

        public Project GetById(Guid id)
        {
            return db.Projects.FindAsync(id).Result;
        }

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

        public async System.Threading.Tasks.Task<bool> AddContactAsync(Guid projectId, Contact contact)
        {
            if (!contact.ProjectId.HasValue) return false;
            var _nowContacts = GetContacts(projectId).Select(t => t.Email);
            if (!_nowContacts.Contains(contact.Email))
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async System.Threading.Tasks.Task AddContactsAsync(Guid projectId, List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                await AddContactAsync(projectId, contact);
            }
        }
    }
}