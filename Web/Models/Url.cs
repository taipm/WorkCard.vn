using CafeT.Html;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Url : BaseObject
    {
        public string Domain { set; get; }
        public string Address { set; get; }
        public string Title { set; get; }
        public string HtmlContent { set; get; }

        public Guid? IssueId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CommentId { set; get; }
        public Guid? StoryId { set; get; }
        public Guid? AnswerId { set; get; }

        public DateTime? LastestCheck { set; get; }

        //public bool IsLive { set; get; }
        public Url() : base() { }
        public Url(string url):base()
        {
            if (url.IsUrl())
            {
                Address = url;
            }
        }

        public void Fetch()
        {
            HtmlContent = this.Address.LoadHtml().Result;
        }
    }
}