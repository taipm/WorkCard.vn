﻿using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.ModelViews
{
    public class YouTubeView
    {
        public string EmbedUrl { set; get; }
        public YouTubeView(string watchUrl)
        {
            string _before = @"<iframe src=";
            string _end = " frameborder=\"0\" allowfullscreen></iframe>";
            string _youtubeLink = string.Empty;
            _youtubeLink = watchUrl.AddBefore(_before).AddAfter(_end).Replace("watch?v=", "embed/");
            EmbedUrl = _youtubeLink;
        }
    }
}