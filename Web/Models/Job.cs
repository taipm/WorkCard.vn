using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CafeT.Text;
using CafeT.Time;
using CafeT.Html;
using CafeT.Objects;

namespace Web.Models
{
    public enum JobStatus
    {
        New,
        Expired
    }

    public class Job:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Content { set; get; }
        public string Message { set; get; } = string.Empty;
        public string Owner { set; get; }
        public double Salary { set; get; } = 0;
        public Address Location { set; get; }
        
        public DateTime? Start { set; get; }
        public DateTime? End { set; get; }
        

        public JobStatus Status { set; get; } = JobStatus.New;

        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }

        public List<string> Tags { set; get; }
       

        public Job():base()
        {
            Tags = new List<string>();
        }

        public bool IsOf(string userName)
        {
            if ((!this.CreatedBy.IsNullOrEmptyOrWhiteSpace() && (this.CreatedBy.ToLower() == userName))
                || (!this.Owner.IsNullOrEmptyOrWhiteSpace() && (this.Owner.ToLower() == userName))) return true;
            return false;
        }

        public bool IsStandard()
        {
            bool _isStandard = this.Start.HasValue && this.End.HasValue;
            return _isStandard;
        }
    }
}