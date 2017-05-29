using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using System.Data;
using CafeT.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Managers
{
    public class IssuesManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<bool> AddQuestionAsync(Question question)
        {
           if(question.IssueId.HasValue)
            {
                db.Questions.Add(question);
                var result = await db.SaveChangesAsync();
                if (result > 0) return true;
                return false;
            }
            return false;
        }
        public async Task<bool> AddIssueAsync(WorkIssue issue)
        {
            bool _result = false;
            UrlManager _urlManager = new UrlManager();
            ContactManager _contactManager = new ContactManager();
            db.Issues.Add(issue);
            try
            {
                await db.SaveChangesAsync();
                _result = true;
            }
            catch (Exception ex)
            {
                return false;
            }


            if (issue.HasInnerMembers())
            {
                var _innerMembers = issue.GetInnerMembers();
                foreach (string _member in _innerMembers)
                {
                    Contact _contact = new Contact();
                    _result =  await _contactManager.AddAsync(_contact);
                    if (!_result) return false;
                }
            }

            if (issue.GetLinks() != null)
            {
                var _links = issue.GetLinks();
                foreach (string link in _links)
                {
                    Url _url = new Url(link);
                    _result = await _urlManager.AddAsync(_url);
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
            _links.AddRange(issue.GetLinks());
            var _questions = GetQuestion(issue);
            foreach(var _question in _questions)
            {
                _links.AddRange(_question.Links);
            }
            return _links.Distinct().AsEnumerable();
        }
    }
}