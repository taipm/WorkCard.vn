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
        public virtual IEnumerable<Story> Stories { set; get; }
        public Project() : base() { }
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