﻿using System;
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
        Done = 3,
        Testing = 4,
        ReOpen = 5
    }

    public enum RepeatType
    {
        None = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Quaterly = 4,
        Yearly = 5
    }

    //public struct IssueMessage
    //{
    //    string 
    //}

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

        public RepeatType Repeat { set; get; } = RepeatType.None;

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
           if(this.Status != IssueStatus.Done)
            {
                if (this.End.HasValue && this.End.Value.IsPrevDay(0)) return true;
            }
            return false;
        }

        public bool IsNoTime()
        {
            if (End == null || !End.HasValue) return true;
            return false;
        }

        public bool IsToday()
        {
            if (End != null && End.HasValue && End.Value.IsToday()) return true;
            return false;
        }
        public bool IsWeekend()
        {
            if (End != null && End.HasValue && End.Value.IsToday()) return true;
            return false;
        }
        public bool IsTomorrow()
        {
            if (End.HasValue && End.Value.IsTomorrow())
                return true;
            return false;
        }
        public bool IsNextDay(int n)
        {
            if (End.HasValue && End.Value.IsNextDay(n))
                return true;
            return false;
        }
        public bool IsPrevDay(int n)
        {
            if (End.HasValue && End.Value.Date.AddDays(n) <= DateTime.Now.Date)
                return true;
            return false;
        }
        public bool IsYesterday()
        {
            if (End.HasValue && End.Value.IsYesterday())
                return true;
            return false;
        }
        
        public bool IsRepeat()
        {
            if(this.Repeat == RepeatType.None)
                return false;
            return true;
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
                Repeat = RepeatType.Daily;
            }
            if (Title.ToLower().Contains("[weekly]"))
            {
                Repeat = RepeatType.Weekly;
            }
            if (Title.ToLower().Contains("[monthly]"))
            {
                Repeat = RepeatType.Monthly;
            }
            if (Title.ToLower().Contains("[quaterly]"))
            {
                Repeat = RepeatType.Quaterly;
            }
            if (Title.ToLower().Contains("[yearly]"))
            {
                Repeat = RepeatType.Yearly;
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

            //BuildTime();
            if (this.IsNoTime())
            {
                if(Times != null && Times.Count > 0)
                {
                }
                else
                {
                    this.SetToday();
                }
            }
        }

        public void SetToday()
        {
            this.Start = DateTime.Today.SetStartWorkingTime();
            this.End = DateTime.Today.SetStartWorkingTime().AddMinutes(30);
        }

        public void SetTomorrow()
        {
            this.Start = DateTime.Today.AddDays(1).SetStartWorkingTime();
            this.End = DateTime.Today.AddDays(1).SetStartWorkingTime().AddMinutes(30);
        }

        public string GetMessage()
        {
            Message = string.Empty;

            if (!this.Start.HasValue)
            {
                Message += "\n Must setup [Start]";
            }

            if (!this.End.HasValue)
            {
                Message += "\n Must setup [End]";
            }
            if (this.IsNoTime())
            {
                Message += "\n Bạn chưa thiết lập thời gian thực thi. Vui lòng cập nhật để chúng tôi có thể hỗ trợ bạn hiệu quả hơn";
            }
            if (this.Status != IssueStatus.Done)
            {
                if (this.End.HasValue && this.End.Value.IsPrevDay(0))
                    Message += "\n Đã quá hạn. Vui lòng cập nhật tiến độ";
            }
            return Message;
        }
    }
}