using System;
using System.Text;

namespace LandoWorks.StringAligner
{
    public class LeftmostStringAligner : StringAlignerBase
    {
        public LeftmostStringAligner(string input) : base(input) {}

        protected override string Align(string[] lines)
        {
            int smallestIndentation = 0;
            
            for(var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
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
