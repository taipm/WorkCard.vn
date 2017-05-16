using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static class SentencesHelper
    {
        #region Words
        public static char[] ExtractSeparators(this string text)
        {
            List<char> separators = new List<char>();
            foreach (char character in text)
            {
                // If the character is not a letter,
                // then by definition it is a separator
                if (!char.IsLetter(character))
                {
                    separators.Add(character);
                }
            }
            return separators.ToArray();
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
        //public static string[] GetHasTags(this string text)
        //{
        //    if (string.IsNullOrWhiteSpace(text))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var matches = Regex.Matches(text, "(\\#\\w+) ");
        //        List<string> _words = new List<string>();
        //        foreach (Match match in matches)
        //        {
        //            _words.Add(match.Value);
        //        }
        //        return _words.ToArray();
        //    }
        //}
        public static string[] ToWords(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                char[] separators = text.ExtractSeparators();
                string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                return words;
            }
        }

        public static string WordsToString(this string[] words)
        {
            string _result = string.Empty;
            foreach(string _word in words)
            {
                _result += " " + _word;
            }
            return _result;
        }
        #endregion

        public static string[] GetSentences(this string text)
        {
            string[] _strSplits = new string[] { ". ", "? ", "! ", "\r\n" };
            return text.ToStandard().Split(_strSplits, StringSplitOptions.None);
        }

        /// <summary>
        /// Authors: Phan Minh Tai
        /// Email: taipm.vn@outlook.com
        /// Date: 19/05/2016
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Count of words in input string</returns>
        public static int GetCountWords(this string input)
        {
            var count = 0;
            try
            {
                // Exclude whitespaces, Tabs and line breaks
                var re = new Regex(@"[^\s]+");
                var matches = re.Matches(input);
                count = matches.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }

        public static int CountOf(this string input, string word)
        {
            int _result = 0;
            int _index = input.IndexOf(word);
            while (_index >= 0)
            {
                input = input.Remove(_index, word.Length);
                _index = input.IndexOf(word);
                _result = _result + 1;
            }
            return _result;
        }

        public static string[] GetSentences(this string text, string word)
        {
            List<string> _sentences = new List<string>();
            //Match Collection for every sentence
            MatchCollection matchSentences =
                Regex.Matches(text, @"([A-Z][^\.!?]*[\.!?])");
            //Alternative pattern :  @"(\S.+?[.!?])(?=\s+|$)"

            //counter for sentences.
            int foundSentenceWithWord = 0;
            foreach (Match sFound in matchSentences)
            {
                foreach (Capture capture in sFound.Captures)
                {
                    string current_sentence = capture.Value;

                    //if you don't want to match for words like 'bank'er  or  'bank'ing
                    //use the word boundary "\b"
                    //change this pattern to   @"\b"+word+@"\b"
                    Match matchWordInSentence =
                        Regex.Match(capture.Value, word, RegexOptions.IgnoreCase);
                    if (matchWordInSentence.Success)
                    {
                        _sentences.Add(current_sentence);
                        foundSentenceWithWord++;
                    }
                }
            }
            return _sentences.ToArray();
        }
    }
}
