using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicKG.HondaPlugins.PredictWrapper
{
    public static class CustomizeTokenizer
    {
        public static Dictionary<string, long> GetVocabs()
        {
            var content = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/Files/chinese_L-12_H-768_A-12-vocab.txt");
            long i = 0;
            var dict = content.Distinct().ToDictionary(k => k.ToLowerInvariant(), v => i++);
            return dict;
        }

        public static List<string> Tokenizer(string inputStr, ref List<string> wordList)
        {
            inputStr = inputStr.ToLowerInvariant();
            var segWords = new List<string>();
            var text = inputStr.ToCharArray();
            var spaced = "";
            foreach (var ch in text)
            {
                if (char.IsPunctuation(ch) || char.IsSymbol(ch) || IsCJKCharacter(ch))
                    spaced += $" {ch} ";
                else if (char.IsWhiteSpace(ch))
                    spaced += " ";
                else if (ch == 0 || ch == 0xfffd || char.IsControl(ch))
                    continue;
                else
                    spaced += ch;
            }

            var input_strs = spaced.TrimEnd().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in input_strs)
            {
                if (wordList.Contains(item))
                {
                    segWords.Add(item);
                    continue;
                }

                int start = 0;

                while (start < item.Length)
                {
                    var sub = "";
                    int stop = item.Length;
                    while (stop > start)
                    {
                        sub = item.Substring(start, stop - start);
                        if (start > 0)
                            sub = "##" + sub;
                        if (wordList.Contains(sub))
                            break;
                        stop -= 1;
                    }
                    if (start == stop)
                        stop += 1;

                    if (!string.IsNullOrWhiteSpace(sub))
                        segWords.Add(sub);

                    start = stop;
                }
            }

            return segWords;
        }

        public static bool IsCJKCharacter(char ch)
        {
            string regex =
            @"\p{IsHangulJamo}|" +
            @"\p{IsCJKRadicalsSupplement}|" +
            @"\p{IsCJKSymbolsandPunctuation}|" +
            @"\p{IsEnclosedCJKLettersandMonths}|" +
            @"\p{IsCJKCompatibility}|" +
            @"\p{IsCJKUnifiedIdeographsExtensionA}|" +
            @"\p{IsCJKUnifiedIdeographs}|" +
            @"\p{IsHangulSyllables}|" +
            @"\p{IsCJKCompatibilityForms}";
            Match match = Regex.Match(Regex.Escape(ch.ToString()), regex);
            return match.Success;
        }
    }
}
