using System;
using System.Text;
using System.Linq;

namespace LandoWorks.StringAlignerStrategyV1
{
    public interface IAlignmentStrategy
    {
        string Align(string input);
    }

    public class LeftAlignmentStrategy : IAlignmentStrategy
    {
        public string Align(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var aligned = string.Join(Environment.NewLine, lines.Skip(1).Take(lines.Length - 2).Select(line => line.TrimStart()));
            return aligned;
        }
    }

    public class LeastIndentedAlignmentStrategy : IAlignmentStrategy
    {
        public string Align(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var validLines = lines.Skip(1).Take(lines.Length - 2);
            var smallestIndentation = validLines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Length - line.TrimStart().Length)
                .DefaultIfEmpty(0)
                .Min();

            var aligned = string.Join(Environment.NewLine, validLines.Select(line => line.Substring(Math.Min(smallestIndentation, line.Length))));
            return aligned;
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
            var lines = input.Split(Environment.NewLine);
            var indicatorLine = lines[1];
            var indicatorSplits = indicatorLine.Split(_markCharacter);

            if (indicatorSplits.Length <= 1)
                throw new Exception($"No indicator character '{_markCharacter}' found in the second line.");

            var smallestIndentation = indicatorSplits[0].Length + 1;
            var aligned = string.Join(Environment.NewLine, lines.Skip(1).Take(lines.Length - 2).Select(line => line.Length > smallestIndentation ? line.Substring(smallestIndentation) : string.Empty));
            return aligned;
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