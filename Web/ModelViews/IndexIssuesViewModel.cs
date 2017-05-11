using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Helpers;
using Web.Models;

namespace Web.ModelViews
{
    public class IndexIssuesViewModel
    {
        public IEnumerable<WorkIssue> Items { get; set; }
        public Pager Pager { get; set; }
    }
}