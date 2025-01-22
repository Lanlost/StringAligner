using System;
using System.Text;

namespace LandoWorks.StringAligner
{
    public static class StringAligner 
    {
        public static LeftJustifiedStringAligner ToLeft(string input) => new(input);
        public static LeftmostStringAligner ToLeftMost(string input) => new(input);
        public static MarkedStringAligner ToMark(char markChar, string input) => new(markChar, input);
    }
}
