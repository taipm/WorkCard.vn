using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CafeT.Text;
using CafeT.Time;
using CafeT.Html;

namespace Web.Models
{
    

    public enum IssueStatus
    {
        New = 0,
        Doing = 1,
        Finished = 2,
        Done = 3
    }

    public class WorkIssue:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Content { set; get; }
        public string Message { set; get; } = string.Empty;
        public string Owner { set; get; }
        
        public DateTime? Start { set; get; }
        public DateTime? End { set; get; }
        public int? TimeToDo { set; get; } = 30; //Default by 30 minutes

        public IssueStatus Status { set; get; } = IssueStatus.New;

        //public bool? IsFinished { set; get; } = false;
        //public bool? IsDone { set; get; } = false;

        public bool? IsDaily { set; get; } = false;
        public bool? IsWeekly { set; get; } = false;
        public bool? IsMonthly { set; get; } = false;
        public bool? IsQuaterly { set; get; }
        public bool? IsYearly { set; get; } = false;

        public Guid? ProjectId { set; get; }
        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }

        public List<string> Tags { set; get; }
        public List<string> Links { set; get; }
        public List<string> Numbers { set; get; }
        public List<DateTime> Times { set; get; }
        public List<string> Emails { set; get; }
        public List<string> HasTags { set; get; }
        public List<string> Members { set; get; }

        public WorkIssue():base()
        {
            HasTags = new List<string>();
            Members = new List<string>();
            Tags = new List<string>();
            Links = new List<string>();
            Emails = new List<string>();
            Times = new List<DateTime>();
        }

        public bool IsStandard()
        {
            bool _isStandard = this.Start.HasValue && this.End.HasValue;
            return _isStandard;
        }

        public bool IsExpired()
        {
            bool _TimeExpiredCondition = (End!=null) && (End.HasValue && End.Value.DayOfYear < DateTime.Now.DayOfYear)
                && (End.Value.Year <= DateTime.Now.Year);
            if (this.Status != IssueStatus.Done && _TimeExpiredCondition)
                return true;
            return false;
        }

        public bool IsToday()
        {
            if (End != null && End.HasValue && End.Value.IsToday()) return true;
            return false;
        }

        public bool IsTomorrow()
        {
            if (End.HasValue && (End.Value.DayOfYear == DateTime.Now.DayOfYear + 1) && End.Value.Year == DateTime.Now.Year)
                return true;
            return false;
        }

        public bool IsYesterday()
        {
            if (End.HasValue && (End.Value.DayOfYear == DateTime.Now.DayOfYear -1) && End.Value.Year == DateTime.Now.Year)
                return true;
            return false;
        }
        
        public bool IsRepeat()
        {
            if (IsDaily.HasValue && IsDaily.Value == true) return true;
            if (IsWeekly.HasValue && IsWeekly.Value == true) return true;
            if (IsMonthly.HasValue && IsMonthly.Value == true) return true;
            if (IsYearly.HasValue && IsYearly.Value == true) return true;
            return false;
        }

        public void AutoAdjust()
        {
            if(Title.IsNullOrEmptyOrWhiteSpace())
            {
                HtmlToText convert = new HtmlToText();
                string _text = convert.Convert(this.Content);
                Title = _text.GetSentences().FirstOrDefault();
            }
            
            if(Title.ToLower().Contains("[daily]"))
            {
                IsDaily = true;
            }
            if (Title.ToLower().Contains("[weekly]"))
            {
                IsWeekly = true;
            }
            if (Title.ToLower().Contains("[monthly]"))
            {
                IsMonthly = true;
            }
            if (Title.ToLower().Contains("[quaterly]"))
            {
                IsQuaterly = true;
            }
            if (Title.ToLower().Contains("[yearly]"))
            {
                IsYearly = true;
            }

            //Members
            string[] _emails = Content.GetEmails();
            if(_emails != null && _emails.Count()>0)
            {
                foreach(string _email in _emails)
                {
                    Members.Add(_email);
                }
            }

            //HasTags
            string[] _hasTags = Content.GetHasTags();
            if (_hasTags != null && _hasTags.Count() > 0)
            {
                foreach (string _hasTag in _hasTags)
                {
                    HasTags.Add(_hasTag);
                }
            }

            //Autofill time
            DateTime[] _times = Content.GetTimes();
            if (_times != null && _times.Count() > 0)
            {
                foreach (var _time in _times)
                {
                    Times.Add(_time);
                }
            }

            //BuildTile();
        }
        public string GetMessage()
        {
            Message = string.Empty;

            if (!this.Start.HasValue)
            {
                Message += "; Must setup [Start]";
            }

            if (!this.End.HasValue)
            {
                Message += "; Must setup [End]";
            }
            return Message;
            //if (!this.Start.HasValue)
            //{
            //    Message += "; Must setup [Start]";
            //}
        }

        //public void NotifyByEmail()
        //{
        //    EmailService _emailService = new EmailService();
        //    try
        //    {
        //        _emailService.SendAsync(this);
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }

    
}