using CafeT.Time;
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
        public IEnumerable<WorkIssue> TodayItems { set; get; }
        public IEnumerable<WorkIssue> NextItems { set; get; } //Net working day
        public IEnumerable<WorkIssue> NextWeekItems { set; get; } //Net working day
        public IEnumerable<WorkIssue> ThisWeekItems { set; get; } //Net working day
        public IEnumerable<WorkIssue> YesterdayItems { set; get; } //Net working day

        public IndexIssuesViewModel(IEnumerable<WorkIssue> issues)
        {
            Items = issues;
            TodayItems = Items.GetToday();
            NextItems = Items.GetInDay(DateTime.Today.AddWorkdays(1));
            YesterdayItems = Items.GetInDay(DateTime.Today.AddWorkdays(-1));
        }
        //public Pager Pager { get; set; }
    }
}