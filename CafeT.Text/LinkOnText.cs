using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Text
{
    
    public static class LinkOnText
    {
        public static string ToHtmlLink(this string url)
        {
            string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.Replace(url, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>")
                .Replace("href=\"www", "href=\"http://www");
        }

        public static string[] GetUrlsWithoutHref(this string text)
        {
            if (text == null || text.Length <= 0) return null;
            //string RegexPattern = @"\b(?:https?://|www\.)\S+\b";
            string RegexPattern = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            MatchCollection matches = Regex.Matches(text, RegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            //// Report on each match.
            int c = 0;
            foreach (Match match in matches)
            {
                MatchList[c] = match.Value;
                c++;
            }

            return MatchList;
        }
        public static string[] GetUrlsWithHref(this string text)
        {
            if (text == null || text.Length <= 0) return null;
            //match.Groups["name"].Value - URL Name
            // match.Groups["url"].Value - URI
            string RegexPattern = @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>";

            // Find matches.
            MatchCollection matches = Regex.Matches(text, RegexPattern, RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // Report on each match.
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.Groups["url"].Value;
                c++;
            }

            return MatchList;
        }
        public static string[] GetUrls(this string text)
        {
            if (text == null || text.Length <= 0) return null;
            List<string> _urls = text.GetUrlsWithHref().ToList();
            List<string> _urls2 = text.GetUrlsWithoutHref().ToList();
            foreach (string _url in _urls2)
            {
                _urls.Add(_url);
            }
            return _urls.ToArray();
        }


        #region Big company links

        public static bool HasYouTubeLink(this string text)
        {
            if(text.GetYouTubeUrls() != null && text.GetYouTubeUrls().Count() > 0)
            {
                return true;
            }
            return false;
        }

        public static string GetYouTubeId(this string youTubeUrl)
        {
            //Setup the RegEx Match and give it 
            Match regexMatch = Regex.Match(youTubeUrl, "^[^v]+v=(.{11}).*",
                               RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                return "http://www.youtube.com/v/" + regexMatch.Groups[1].Value + "&hl=en&fs=1";
            }
            return youTubeUrl;
        }

        public static string[] GetYouTubeUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("youtube.com"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }
        public static string[] GetGoogleDriveUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://drive.google.com/drive"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }
        public static string[] GetDropboxUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://www.dropbox.com/"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }
        public static string[] GetMicrosoftDriveUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://onedrive.live.com"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }
        #endregion
    }
}
