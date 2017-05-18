using CafeT.Objects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Project : BaseObject
    {
        public string Title { set; get; }
        public string Description { set; get; }

        public virtual IEnumerable<WorkIssue> Issues { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Document> Documents { set; get; }
        public virtual IEnumerable<Contact> Contacts { set; get; }
        public virtual IEnumerable<Story> Stories { set; get; }
        public Project() : base() { }
        public Project(string Name) : base() { Title = Name; }

        public IEnumerable<string> GetUrls()
        {
            //Dictionary<string, string> _dict = new Dictionary<string, string>();
            List<string> _urls = new List<string>();
            var _fields = this.Fields();
            foreach(var item in _fields)
            {
                //_dict.Add(item.Key, item.Value.ToJson());
                _urls.AddRange(item.Value.ToJson().GetUrls().ToList());
            }
            return _urls;
        }
    }

    public class Story : BaseObject
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public Guid? ProjectId { set; get; }
        public Story() : base() { }
    }
    

    public class Answer : BaseObject
    {
        public string Content { set; get; }
        public Guid QuestionId { set; get; }
        public Answer() : base() { }
    }

    public class Comment : BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? IssueId { set; get; }
        public Guid? QuestionId { set; get; }

        public Comment() : base() { }
    }
}