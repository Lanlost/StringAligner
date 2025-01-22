namespace LandoWorks.StringAlignV1
{
    [Flags]
    public enum StringLiteralType
    {
        LeftAligned = 1,
        FirstLine = 2,   
    }

    public class StringAlign
    {
        public StringLiteralType @Type { get; private set; }
        public string AlignedString { get; private set; } = string.Empty;
        public string OriginalString { get; private set; } = string.Empty;
        
        private StringAlign() {}
        
        public static StringAlign ToMark(char indicator, string input)
        {
            var stringLiteral = new StringAlign()
            {
                @Type = StringLiteralType.LeftAligned,
                OriginalString = input,
                AlignedString = string.Empty,
            };
            
            if (input == null)
                throw new Exception(nameof(input) + " cannot be null or whitespace.");
            
            var lines = input.Split(Environment.NewLine);
            var indicatorLineSplits = lines[1].Split(indicator);
            
            if (indicatorLineSplits.Length == 1)
                throw new Exception("No indicator character found.");

            var smallestIndentation = indicatorLineSplits.First().Length + 1;
            
            stringLiteral.AlignedString += $@"Smallest indentation: { smallestIndentation }" + Environment.NewLine;
            
            for(var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmedLine = line.TrimStart();

                if (index == lines.Length - 1 && string.IsNullOrWhiteSpace(trimmedLine))
                    break;

                var substr = line.Substring(smallestIndentation);
                stringLiteral.AlignedString += $@"Line #{ index+1 }: ""{ substr }"" [Spaces Removed: { smallestIndentation }]" + Environment.NewLine;
            }
            
            return stringLiteral;
        }
        
        public static StringAlign ToLeastIndented(string input)
        {
            var stringLiteral = new StringAlign()
            {
                @Type = StringLiteralType.LeftAligned,
                OriginalString = input,
                AlignedString = string.Empty,
            };
            
            if (input == null)
                throw new Exception(nameof(input) + " cannot be null or whitespace.");
            
            var lines = input.Split(Environment.NewLine);
            int smallestIndentation = 0;
            
            for(var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmedLine = line.TrimStart();

                if (index == lines.Length - 1 && string.IsNullOrWhiteSpace(trimmedLine))
                    break;

                var cnt = line.Length - trimmedLine.Length;
                
                if (index == 1)
                    smallestIndentation = cnt;
                else if (cnt < smallestIndentation)
                    smallestIndentation = cnt;
                
                stringLiteral.AlignedString += $@"Smallest indentation: { smallestIndentation }" + Environment.NewLine;
            }
            
            for(var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmedLine = line.TrimStart();

                if (index == lines.Length - 1 && string.IsNullOrWhiteSpace(trimmedLine))
                    break;

                var substr = line.Substring(smallestIndentation);
                stringLiteral.AlignedString += $@"Line #{ index+1 }: ""{ substr }"" [Spaces Removed: { smallestIndentation }]" + Environment.NewLine;
            }
            
            return stringLiteral;
        }
        
        public static StringAlign ToLeft(string input)
        {
            var stringLiteral = new StringAlign()
            {
                @Type = StringLiteralType.LeftAligned,
                OriginalString = input,
                AlignedString = string.Empty,
            };
            
            if (input == null)
                throw new Exception(nameof(input) + " cannot be null or whitespace.");
            
            var lines = input.Split(Environment.NewLine);

            for(var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmedLine = line.TrimStart();

                if (index == lines.Length - 1 && string.IsNullOrWhiteSpace(trimmedLine))
                    break;

                var cnt = line.Length - trimmedLine.Length;
                var substr = line.Substring(cnt);

                stringLiteral.AlignedString += $@"Line #{ index+1 }: ""{ substr }"" [Spaces Removed: { cnt }]" + Environment.NewLine;
            }
            
            return stringLiteral;
        }
        
        public override string ToString()
        {
            return AlignedString;
        }
    }
}

