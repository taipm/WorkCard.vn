using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static class EmailOnText
    {
        public static string[] GetEmails(this string str)
        {
            if (str == null || str.Length <= 0) return null;

            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = Regex.Matches(str, RegexPattern, RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // add each match
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }

            return MatchList;
        }

        public static string[] GetHasTags(this string text)
        {
            //string text = "This is a string that #contains a hashtag!";
            if (text == null || text.Length <= 0) return null;
            List<string> _hasTags = new List<string>();
            var regex = new Regex(@"(?<=#)\w+");
            var matches = regex.Matches(text);
            foreach (Match m in matches)
            {
                _hasTags.Add(m.Value);
            }
            return _hasTags.ToArray();
        }
    }
}
