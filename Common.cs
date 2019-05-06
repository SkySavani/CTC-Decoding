using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Common
    {
        public static List<string> GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b$%");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToList();
        }

        public static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }

        public static List<string> GetAlphaWords(List<string> input)
        {
            Regex r = new Regex("^[a-zA-Z]*$");
            List<string> words = new List<string>();
            foreach (var v in input)
            {
                if (r.Match(v).Success)
                {
                    words.Add(v);
                }
            }

            return words;
        }

        public static List<int> wordToLabelSeq(string w, string classes)
        {
            List<int> res = new List<int>();
            foreach (var c in w)
            {
                res.Add(classes.IndexOf(c));
            }

            return res;
        }

        public static List<int> extendByBlanks(List<int> seq, int b)
        {
            List<int> res = new List<int>() { b };
            foreach (var s in seq)
            {
                res.Add(s);
                res.Add(b);
            }
            return res;
        }
    }
}
