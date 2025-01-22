using System;
using System.Text;

namespace LandoWorks.StringAligner
{
    public abstract class StringAlignerBase
    {
        public string OriginalString { get; protected set; }
        public string Value { get; protected set; }
        
        public StringAlignerBase(string input, bool skipProcessing = false)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input), $"Value of { nameof(input) } cannot be null or whitespace.");

            OriginalString = input;
            Value = string.Empty;

            if (skipProcessing) 
                return;
            
            var lines = GetLines(input);
            Value = Align(lines);
        }

        public override string ToString() 
        {
            return Value;
        }

        protected virtual string[] GetLines(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new Exception(nameof(input) + " cannot be null or empty.");

            var lines = input.Split(Environment.NewLine);

            // Skip the first line (header)
            var startLineNumber = 1;

            // Skip the last line if it's whitespace
            var endLineNumber = string.IsNullOrWhiteSpace(lines[^1]) 
                ? lines.Length - 1 
                : lines.Length;

            return lines[startLineNumber..endLineNumber];
        }

        protected virtual string ProcessLines(IEnumerable<string> lines, Func<string, string> lineProcessor)
        {
            var builder = new StringBuilder();

            foreach (var line in lines)
            {
                builder.AppendLine(lineProcessor(line));
            }

            return builder.ToString();
        }

        protected abstract string Align(string[] lines);
    }
}
