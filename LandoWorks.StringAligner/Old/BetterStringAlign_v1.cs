using System;
using System.Text;

namespace LandoWorks.BetterStringAlignV1
{
    [Flags]
    public enum StringAlignmentType
    {
        Left = 1,
        LeastIndented = 2,
        Mark = 4,
    }

    public interface IMarkedStringAlign : IStringAligned
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
        public static IMarkedStringAlign ToMark(char markChar, string input) => MarkedStringAlign.Create(markChar, input);

        protected static string[] SplitLines(string input) => input.Split(Environment.NewLine);

        protected static int FindSmallestIndentation(string[] lines)
        {
            return lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Length - line.TrimStart().Length)
                .DefaultIfEmpty(0)
                .Min();
        }

        protected static string AlignLines(string[] lines, int indentation)
        {
            var builder = new StringBuilder();

            foreach (var line in lines)
            {
                var trimmedLine = line.TrimStart();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                builder.AppendLine(line.Substring(indentation));
            }

            return builder.ToString();
        }

        public override string ToString() => AlignedString;
    }

    public class MarkedStringAlign : StringAlign, IMarkedStringAlign
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
            var alignedString = AlignLines(lines.Skip(1).ToArray(), smallestIndentation);

            return new MarkedStringAlign(mark, input) { AlignedString = alignedString };
        }
    }

    public class LeastIndentedStringAlign : StringAlign
    {
        private LeastIndentedStringAlign(string input) : base(input, StringAlignmentType.LeastIndented) { }

        public static StringAlign Create(string input)
        {
            var lines = SplitLines(input);
            var smallestIndentation = FindSmallestIndentation(lines);
            var alignedString = AlignLines(lines, smallestIndentation);
            return new LeastIndentedStringAlign(input) { AlignedString = alignedString };
        }
    }

    public class LeftAlignedStringAlign : StringAlign
    {
        private LeftAlignedStringAlign(string input) : base(input, StringAlignmentType.Left) { }

        public static StringAlign Create(string input)
        {
            var lines = SplitLines(input);
            var alignedString = AlignLines(lines, 0); // No indentation for left alignment
            return new LeftAlignedStringAlign(input) { AlignedString = alignedString };
        }
    }
}
