using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace LandoWorks.StringAlignV4
{
    public interface IMarkedAlignedString : IAlignedString
    {
        public char MarkCharacter { get; }
    }

    public interface IAlignedString
    {
        public string Value { get; }
        public string OriginalString { get; }
    }
    
    public class LeftAlignedStringAlign : StringAlign, IAlignedString
    {
        protected LeftAlignedStringAlign(string input, string value) : base(input, value)
        {
        }
        
        public static LeftAlignedStringAlign Create(string input)
        {
            var output = AlignLines
            (
                lines: GetValidLines(input), 
                lineProcessor: line => line.TrimStart()
            );

            return new LeftAlignedStringAlign(input, output);
        }
    }
    
    public class LeastIndentedStringAlign : StringAlign, IAlignedString
    {
        protected LeastIndentedStringAlign(string input, string value) : base(input, value)
        {
        }

        public static LeastIndentedStringAlign Create(string input)
        {
            var validLines = GetValidLines(input);
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

            var output = AlignLines
            (
                lines: validLines, 

                lineProcessor: line => smallestIndentation <= line.Length 
                    ? line.Substring(smallestIndentation) 
                    : string.Empty
            );

            return new LeastIndentedStringAlign(input, output);
        }
    }

    public class MarkedStringAlign : StringAlign, IMarkedAlignedString
    {
        public char MarkCharacter { get; protected set; }
        
        protected MarkedStringAlign(char markCharacter, string input, string value) : base(input, value)
        {
            MarkCharacter = markCharacter;
        }
        
        public static MarkedStringAlign Create(char mark, string input)
        {
            var validLines = GetValidLines(input);
            
            var indicatorLineSplits = validLines[0].Split(mark);

            if (indicatorLineSplits.Length == 1)
                throw new Exception("No indicator character found.");

            var smallestIndentation = indicatorLineSplits[0].Length + 1;
            
            var output = AlignLines
            (
                lines: validLines, 

                lineProcessor: line => smallestIndentation <= line.Length 
                    ? line.Substring(smallestIndentation)
                    : string.Empty
            );

            return new MarkedStringAlign(mark, input, output);
        }
    }
    
    public class StringAlign
    {
        public string Value { get; protected set; }
        public string OriginalString { get; protected set; } 
        
        protected StringAlign(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input), $"Value of { nameof(input) } cannot be null or whitespace.");

            OriginalString = input;
            Value = string.Empty;
        }
        
        protected StringAlign(string input, string value)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input), $"Value of { nameof(input) } cannot be null or empty.");

            OriginalString = input;
            Value = value ?? string.Empty;
        }
       
        public static LeftAlignedStringAlign ToLeft(string input) => LeftAlignedStringAlign.Create(input);
        public static LeastIndentedStringAlign ToLeftMost(string input) => LeastIndentedStringAlign.Create(input);
        public static MarkedStringAlign ToMark(char markChar, string input) => MarkedStringAlign.Create(markChar, input);

        public override string ToString() => Value;

        protected static string[] GetValidLines(string input)
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

        protected static string AlignLines(IEnumerable<string> lines, Func<string, string> lineProcessor)
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
