﻿using CafeT.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    
    public class Url:BaseObject
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
        public Url(string url)
        {
            if(url.IsUrl())
            {
                Address = url;
                //Domain = url.GetDomain()
            }
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
        public Guid? IssueId { set; get; }
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