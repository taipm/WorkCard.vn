using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using System.Data;

namespace Web.Managers
{
    public class IssuesManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool AddIssue(WorkIssue issue)
        {
            bool _result = false;
            UrlManager _urlManager = new UrlManager();
            ContactManager _contactManager = new ContactManager();

            //using (var transaction = new System.Transactions.TransactionScope())
            //{
            //    // DBC = Database Command

            //    // create the database context
            //    var database = new DatabaseContext();

            //    // search for the user with id #1
            //    // DBC: BEGIN TRANSACTION
            //    // DBC: select * from [User] where Id = 1
            //    var userA = database.Users.Find(1);
            //    // DBC: select * from [User] where Id = 2
            //    var userB = database.Users.Find(2);
            //    userA.Name = "Admin";

            //    // DBC: update User set Name = "Admin" where Id = 1
            //    database.SaveChanges();

            //    userB.Age = 28;
            //    // DBC: update User set Age = 28 where Id = 2
            //    database.SaveChanges();

            //    // DBC: COMMIT TRANSACTION
            //    transaction.Complete();
            //}
            db.Issues.Add(issue);
            try
            {
                db.SaveChangesAsync();
                _result = true;
            }
            catch(Exception ex)
            {
                return false;
            }
            

            if (issue.HasInnerMembers())
            {
                var _innerMembers = issue.GetInnerMembers();
                foreach (string _member in _innerMembers)
                {
                    Contact _contact = new Contact();
                    _result = _contactManager.Add(_contact);
                    if (!_result) return false;
                }
            }

            if (issue.GetUrls() != null)
            {
                var _links = issue.GetUrls();
                foreach (string link in _links)
                {
                    Url _url = new Models.Url(link);
                    _result = _urlManager.Add(_url);
                    if (!_result) return false;
                }
            }
            return _result;
        }

        public IEnumerable<Question> GetQuestion(WorkIssue issue)
        {
            return db.Questions.Where(t => t.IssueId.HasValue && t.IssueId == issue.Id).AsEnumerable();
        }

        public IEnumerable<string> GetLinks(WorkIssue issue)
        {
            List<string> _links = new List<string>();
            _links.AddRange(issue.GetUrls());
            var _questions = GetQuestion(issue);
            foreach(var _question in _questions)
            {
                _links.AddRange(_question.Links);
            }
            return _links.Distinct().AsEnumerable();
        }
    }
}