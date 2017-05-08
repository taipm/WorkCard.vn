using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Document:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Path { set; get; }
    }
}