using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class BaseObject : CafeT.EF6.Objects.BaseObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { set; get; }
        [ScaffoldColumn(false)]
        public DateTime? UpdatedDate { set; get; }
        [ScaffoldColumn(false)]
        public string UpdatedBy { set; get; }
        [ScaffoldColumn(false)]
        public string CreatedBy { set; get; }

        public BaseObject()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }
}