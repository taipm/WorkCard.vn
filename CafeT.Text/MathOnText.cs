using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static class MathOnText
    {
        public static string[] ExtractAllMathText(this string text)
        {
            string[] _commands = text.ExtractAllBetween("$$", "$$");
            return _commands;
        }

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

        public static char[] ExtractOperators(string expression)
        {
            string operatorCharacters = "+-*/";
            List<char> operators = new List<char>();
            foreach (char c in expression)
            {
                if (operatorCharacters.Contains(c))
                {
                    operators.Add(c);
                }
            }
            return operators.ToArray();
        }
    }
}
