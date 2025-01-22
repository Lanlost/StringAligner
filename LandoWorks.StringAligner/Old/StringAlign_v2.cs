namespace LandoWorks.StringAlignV2
{
    [Flags]
    public enum StringAlignmentType
    {
        Left = 1,
        LeastIndented = 2,
        Mark = 4,
    }

    public interface IMarkedAlignedString : IAlignedString
    {
        public char MarkCharacter { get; }
    }

    public interface IAlignedString
    {
        public StringAlignmentType @Type { get; }
        public string AlignedString { get; }
        public string OriginalString { get; }
    }
    
    public class MarkedStringAlign : StringAlign, IMarkedAlignedString
    {
        public char MarkCharacter { get; protected set; }
        
        protected MarkedStringAlign(char markCharacter, String input, StringAlignmentType type) 
            : base(input, type)
        {
            this.MarkCharacter = markCharacter;
        }
        
        public static MarkedStringAlign Create(char mark, string input)
        {
            var stringLiteral = new MarkedStringAlign(mark, input, StringAlignmentType.Mark);
            
            if (input == null)
                throw new Exception(nameof(input) + " cannot be null or whitespace.");
            
            var lines = input.Split(Environment.NewLine);
            var indicatorLineSplits = lines[1].Split(mark);
            
            if (indicatorLineSplits.Length == 1)
                throw new Exception("No indicator character found.");

            var smallestIndentation = indicatorLineSplits.First().Length + 1;
            
            for(var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmedLine = line.TrimStart();

                if (index == lines.Length - 1 && string.IsNullOrWhiteSpace(trimmedLine))
                    break;

                var substr = line.Substring(smallestIndentation);
                stringLiteral.AlignedString += substr + Environment.NewLine;
            }
            
            return stringLiteral;
        }
    }
    
    public class StringAlign
    {
        public StringAlignmentType @Type { get; protected set; }
        public string AlignedString { get; protected set; }
        public string OriginalString { get; protected set; }
        
        protected StringAlign(String input, StringAlignmentType type) 
        {
            OriginalString = input;
            AlignedString = String.Empty;
            @Type = type;
        }
        
        public static MarkedStringAlign ToMark(char markChar, string input)
        {
            return MarkedStringAlign.Create(markChar, input);
        }
        
        public static StringAlign ToLeastIndented(string input)
        {
            var stringLiteral = new StringAlign(input, StringAlignmentType.LeastIndented);
            
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
            }                
            
            for(var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmedLine = line.TrimStart();

                if (index == lines.Length - 1 && string.IsNullOrWhiteSpace(trimmedLine))
                    break;

                var substr = line.Substring(smallestIndentation);
                stringLiteral.AlignedString += substr + Environment.NewLine;
            }
            
            return stringLiteral;
        }
        
        public static StringAlign ToLeft(string input)
        {
            var stringLiteral = new StringAlign(input, StringAlignmentType.Left);
            
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

                stringLiteral.AlignedString += substr + Environment.NewLine;
            }
            
            return stringLiteral;
        }
        
        public override string ToString()
        {
            return AlignedString;
        }
    }
}
