using System;
using System.Text;

namespace LandoWorks.BetterStringAlignV3
{
    [Flags]
    public enum StringAlignmentType
    {
        Left = 1,
        LeastIndented = 2,
        Mark = 4,
    }

    public interface IMarkedStringAligned : IStringAligned
    {
        char MarkCharacter { get; }
    }

    public interface IStringAligned
    {
        StringAlignmentType Type { get; }
        string AlignedString { get; }
        string OriginalString { get; }
    }

    public abstract class StringAlign : IStringAligned
    {
        public StringAlignmentType Type { get; protected set; }
        public string AlignedString { get; protected set; } = string.Empty;
        public string OriginalString { get; protected set; }

        protected StringAlign(string input, StringAlignmentType type)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{nameof(input)} cannot be null or whitespace.", nameof(input));

            OriginalString = input;
            Type = type;
        }

        public static IStringAligned ToLeft(string input) => LeftAlignedStringAlign.Create(input);
        public static IStringAligned ToLeastIndented(string input) => LeastIndentedStringAlign.Create(input);
        public static IMarkedStringAligned ToMark(char markChar, string input) => MarkedStringAlign.Create(markChar, input);

        protected static string[] SplitLines(string input) => input.Split(Environment.NewLine);

        protected static string AlignLines(string[] lines, Func<string, string> lineProcessor)
        {
            var builder = new StringBuilder();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                builder.AppendLine(lineProcessor(lines[i]));
            }

            // Add the last line if it's not whitespace
            if (!string.IsNullOrWhiteSpace(lines[^1]))
            {
                builder.AppendLine(lineProcessor(lines[^1]));
            }

            return builder.ToString();
        }

        public override string ToString() => AlignedString;
    }

    public class LeftAlignedStringAlign : StringAlign
    {
        private LeftAlignedStringAlign(string input) : base(input, StringAlignmentType.Left) { }

        public static StringAlign Create(string input)
        {
            var lines = SplitLines(input);
            var alignedString = AlignLines(lines, line => line.TrimStart());
            return new LeftAlignedStringAlign(input) { AlignedString = alignedString };
        }
    }

    public class LeastIndentedStringAlign : StringAlign
    {
        private LeastIndentedStringAlign(string input) : base(input, StringAlignmentType.LeastIndented) { }

        public static StringAlign Create(string input)
        {
            var lines = SplitLines(input);
            
            var smallestIndentation = lines.Skip(1).SkipLast(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Length - line.TrimStart().Length)
                .DefaultIfEmpty(0)
                .Min();

            var alignedString = AlignLines(lines, line => line.Substring(Math.Min(smallestIndentation, line.Length)));
            return new LeastIndentedStringAlign(input) { AlignedString = alignedString };
        }
    }

    public class MarkedStringAlign : StringAlign, IMarkedStringAligned
    {
        public char MarkCharacter { get; private set; }

        private MarkedStringAlign(char markCharacter, string input)
            : base(input, StringAlignmentType.Mark)
        {
            MarkCharacter = markCharacter;
        }

        public static MarkedStringAlign Create(char mark, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{nameof(input)} cannot be null or whitespace.", nameof(input));

            var lines = SplitLines(input);
            if (lines.Length < 2)
                throw new Exception("Input must contain at least two lines.");

            var indicatorLine = lines[1];
            var indicatorSplits = indicatorLine.Split(mark);
            if (indicatorSplits.Length <= 1)
                throw new Exception($"No indicator character '{mark}' found in the second line.");

            var smallestIndentation = indicatorSplits[0].Length + 1;
            var alignedString = AlignLines(lines, line => line.Length > smallestIndentation
                ? line.Substring(smallestIndentation)
                : string.Empty);

            return new MarkedStringAlign(mark, input) { AlignedString = alignedString };
        }
    }
}
