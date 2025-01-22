using System;
using System.Text;

namespace LandoWorks.StringAligner
{
    public class MarkedStringAligner : StringAlignerBase
    {
        public char MarkCharacter { get; protected set; }

        public MarkedStringAligner(char markCharacter, string input) : base(input, skipProcessing: true)
        {
            MarkCharacter = markCharacter;

            // Since we skipped processing we have to do it ourselves.
            var lines = GetLines(input);
            Value = Align(lines);
        }

        protected override string Align(string[] lines)
        {
            var indicatorLineSplits = lines[0].Split(MarkCharacter);

            if (indicatorLineSplits.Length == 1)
                throw new Exception("No Indicator character '" + MarkCharacter + "' found in first line.");

            var smallestIndentation = indicatorLineSplits[0].Length + 1;
            
            return ProcessLines
            (
                lines, 

                lineProcessor: line => smallestIndentation <= line.Length 
                    ? line.Substring(smallestIndentation)
                    : string.Empty
            );
        }
    }
}
