using System;
using System.Text;
using System.Linq;
using LandoWorks.StringAlignerV2;

namespace LandoWorks.StringAlignerV2
{
    internal static class Shared
    {
        public static string[] GetValidLines(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new Exception(nameof(input) + " cannot be null or empty.");

            var lines = input.Split(Environment.NewLine);

            if (lines.Length < 2)
                throw new Exception(nameof(input) + " must contain at least one line after the header.");

            // Skip the first line, which is the 'header'.
            var startLineNumber = 1; 
            
            // Skip the last line if it's whitespace.
            var endLineNumber = string.IsNullOrWhiteSpace(lines[^1]) 
                ? lines.Length - 1 
                : lines.Length; 

            return lines[startLineNumber .. endLineNumber];
        }

        public static string AlignLines(IEnumerable<string> lines, Func<string, string> lineProcessor)
        {
            var builder = new StringBuilder();

            foreach (var line in lines)
            {
                builder.AppendLine(lineProcessor(line));
            }

            return builder.ToString();
        }
    }
}

namespace LandoWorks.StringAlignerV2
{
    public interface IAlignmentStrategy
    {
        string Align(string input);
    }

    public class LeftAlignmentStrategy : IAlignmentStrategy
    {
        public string Align(string input)
        {
            var output = Shared.AlignLines
            (
                lines: Shared.GetValidLines(input), 
                lineProcessor: line => line.TrimStart()
            );

            return output;
        }
    }

    public class LeastIndentedAlignmentStrategy : IAlignmentStrategy
    {
        public string Align(string input)
        {
            var validLines = Shared.GetValidLines(input);
            int smallestIndentation = 0;
            
            for(var index = 0; index < validLines.Length; index++)
            {
                var line = validLines[index];
                var trimmedLine = line.TrimStart();

                // We don't want to align to the left because of an empty line.
                if (trimmedLine == string.Empty)
                    continue;

                var cnt = line.Length - trimmedLine.Length;
                
                if (index == 0)
                    smallestIndentation = cnt;
                else if (cnt < smallestIndentation)
                    smallestIndentation = cnt;
            }                

            var output = Shared.AlignLines
            (
                lines: validLines, 

                lineProcessor: line => smallestIndentation <= line.Length 
                    ? line.Substring(smallestIndentation) 
                    : string.Empty
            );

            return output;
        }
    }

    public class MarkedAlignmentStrategy : IAlignmentStrategy
    {
        private readonly char _markCharacter;

        public MarkedAlignmentStrategy(char markCharacter)
        {
            _markCharacter = markCharacter;
        }

        public string Align(string input)
        {
            var validLines = Shared.GetValidLines(input);
            
            var indicatorLineSplits = validLines[0].Split(_markCharacter);

            if (indicatorLineSplits.Length == 1)
                throw new Exception("No indicator character found.");

            var smallestIndentation = indicatorLineSplits[0].Length + 1;
            
            var output = Shared.AlignLines
            (
                lines: validLines, 

                lineProcessor: line => smallestIndentation <= line.Length 
                    ? line.Substring(smallestIndentation)
                    : string.Empty
            );

            return output;
        }
    }

    public class StringAligner
    {
        private readonly IAlignmentStrategy _alignmentStrategy;

        public StringAligner(IAlignmentStrategy alignmentStrategy)
        {
            _alignmentStrategy = alignmentStrategy;
        }

        public string Align(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or whitespace.");

            return _alignmentStrategy.Align(input);
        }
    }
}