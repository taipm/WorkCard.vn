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
        //public static string[] GetSentencesHasPair(this string text)
        //{
            
        //}
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

        //public static string[] GetSentencesHasTime(this string text)
        //{
        //    List<string> _sentences = new List<string>();
        //    //Match Collection for every sentence
        //    MatchCollection matchSentences =
        //        Regex.Matches(text, @"([A-Z][^\.!?]*[\.!?])");
        //    //Alternative pattern :  @"(\S.+?[.!?])(?=\s+|$)"

        //    //counter for sentences.
        //    int foundSentenceWithWord = 0;
        //    foreach (Match sFound in matchSentences)
        //    {
        //        foreach (Capture capture in sFound.Captures)
        //        {
        //            string current_sentence = capture.Value;

        //            //if you don't want to match for words like 'bank'er  or  'bank'ing
        //            //use the word boundary "\b"
        //            //change this pattern to   @"\b"+word+@"\b"
        //            Match matchWordInSentence =
        //                Regex.Match(capture.Value, word, RegexOptions.IgnoreCase);
        //            if (matchWordInSentence.Success)
        //            {
        //                _sentences.Add(current_sentence);
        //                foundSentenceWithWord++;
        //            }
        //        }
        //    }
        //    return _sentences.ToArray();
        //}

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
