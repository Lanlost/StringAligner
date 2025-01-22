using System;
using System.Text;

namespace LandoWorks.StringAligner
{
    public static class StringLiteralExtensions
    {
        public static string ToLeft(this string input) => StringAligner.ToLeft(input).Value;
        public static string ToLeftMost(this string input) => StringAligner.ToLeftMost(input).Value;
        public static string ToMark(this string input, char markChar) => StringAligner.ToMark(markChar, input).Value;
    }
}
