using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    //public struct TextStructure
    //{
    //    int CountOfWords;
    //    int CountOfSentences;
    //    int CountOfEmails;
    //    int CountOfLinks;
    //}

    public class BaseObject : CafeT.EF6.Objects.BaseObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? UpdatedDate { set; get; }
        public string UpdatedBy { set; get; }
        public string CreatedBy { set; get; }

        //public string Message { set; get; }

        public BaseObject()
        {
            CreatedDate = DateTime.Now;
        }
    }

    public class Project : BaseObject
    {
        public string Title { set; get; }
        public string Description { set; get; }

        public virtual IEnumerable<WorkIssue> Issues { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Document> Documents { set; get; }
        public virtual IEnumerable<Story> Stories { set; get; }
    }

    public class Story : BaseObject
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public Guid? ProjectId { set; get; }
    }
    public class Question : BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public virtual IEnumerable<Answer> Answers { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? StoryId { set; get; }
    }

    public class Answer : BaseObject
    {
        public string Content { set; get; }
        public Guid QuestionId { set; get; }
    }

    public class Comment : BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? IssueId { set; get; }
        public Guid? QuestionId { set; get; }
    }
}