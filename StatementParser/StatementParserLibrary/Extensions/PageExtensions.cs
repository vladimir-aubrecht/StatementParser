using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Graphics.Operations.TextState;

namespace StatementParserLibrary.Extensions
{
    internal static class PageExtensions
    {
        private static Word GetWordByIndex(this Page page, long wordIndex)
        {
            long index = 0;
            foreach (var currentWord in page.GetWords())
            {
                if (index == wordIndex)
                {
                    return currentWord;
                }

                index++;
            }

            return null;
        }

        private static IList<long> GetWordIndexes(this Page page, string word)
        {
            var indexes = new List<long>();

            var index = 0;
            foreach (var currentWord in page.GetWords())
            {
                if (currentWord.Text == word)
                {
                    indexes.Add(index);
                }

                index++;
            }

            return indexes;
        }

        private static bool IsWordSequanceStartingAtIndex(this Page page, long index, string text)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var result = true;
            int i = 0;
            foreach (var word in words)
            {
                if (page.GetWordByIndex(index + i).Text != word)
                {
                    result = false;
                }

                i++;
            }

            return result;
        }

        private static (long, long) GetTextBounderies(this Page page, string text, long startSinceWordIndex)
        {
            var index = 0;
            foreach (var word in page.GetWords())
            {
                if (startSinceWordIndex <= index)
                {
                    continue;
                }

                if (page.IsWordSequanceStartingAtIndex(index, text))
                {
                    return (index, index + text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length - 1);
                }

                index++;
            }

            return (-1, -1);
        }

        public static string GetWordsByIndexRelativeToText(this Page page, string startText, params int[] indexes)
        {
            var amountOfWords = startText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            var startTextBoundaries = page.GetTextBounderies(startText, 0);

            string output = null;
            foreach (var index in indexes)
            {
                var skipper = index >= 0 ? amountOfWords : 0;
                var wordIndex = startTextBoundaries.Item2 + index + skipper - 1;

                output = page.GetWordByIndex(wordIndex).Text;
            }

            return output;
        }

        public static string GetTextByTextBoundaries(this Page page, string startText, string endText)
        {
            var startTextBoundaries = page.GetTextBounderies(startText, 0);
            var endTextBoundaries = page.GetTextBounderies(endText, 0);

            if (startTextBoundaries.Item2 == -1 || endTextBoundaries.Item1 == -1)
            {
                return null;
            }

            string outputText = null;
            int i = -1;
            foreach (var word in page.GetWords())
            {
                i++;

                if (i <= startTextBoundaries.Item2)
                {
                    continue;
                }

                if (i >= endTextBoundaries.Item1)
                {
                    continue;
                }

                outputText += $" {word.Text}";
            }

            outputText = outputText.TrimStart();

            return outputText;
        }
    }
}
