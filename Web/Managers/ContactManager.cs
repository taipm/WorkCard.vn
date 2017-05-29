using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class ContactManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async System.Threading.Tasks.Task<bool> AddContactsAsync(List<Contact> contacts)
        {
            bool result = true;
            foreach(var contact in contacts)
            {
                result =  result && await AddAsync(contact);
                //if (!result) return false;
            }
            return result;
        }


        public async System.Threading.Tasks.Task<bool> AddAsync(Contact contact)
        {
            var _myContacts = db.Contacts.Where(t => t.UserName == contact.UserName).Select(t => t.Email);
            if (!_myContacts.Contains(contact.Email))
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Contact GetById(Guid contactId)
        {
            return db.Contacts.FindAsync(contactId).Result;
        }

        public IEnumerable<WorkIssue> GetAllIssues(Guid contactId)
        {
            List<WorkIssue> _results = new List<WorkIssue>();
            Contact _contact = GetById(contactId);
            string _email = _contact.Email;
            var _issues = db.Issues;
            foreach(var _issue in _issues)
            {
                if(_issue.IsOf(_email))
                    _results.Add(_issue);
            }
            return _results;
        }
        public IEnumerable<Contact> GetContactsOfIssue(Guid issueId)
        {
            return db.Contacts.Where(t => t.IssueId == issueId);
        }
        public IEnumerable<WorkIssue> GetTodayIssues(Guid contactId)
        {
            return GetAllIssues(contactId).Where(t => t.IsToday()).AsEnumerable();
        }
    }
}