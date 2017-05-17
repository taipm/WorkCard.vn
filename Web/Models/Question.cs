using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Question : BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public virtual IEnumerable<Answer> Answers { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? StoryId { set; get; }
        public Guid? IssueId { set; get; }
        public List<string> Links
        {
            get
            {
                List<string> _links = new List<string>();
                _links.AddRange(this.Content.GetUrls().ToList());
                if (!this.Title.IsNullOrEmptyOrWhiteSpace())
                    _links.AddRange(this.Title.GetUrls().ToList());
                return _links.Distinct().ToList();
            }
        }
        public Question() : base() { }
    }
}