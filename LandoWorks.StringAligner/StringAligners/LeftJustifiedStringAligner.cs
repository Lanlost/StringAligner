using System;
using System.Text;

namespace LandoWorks.StringAligner
{
    public class LeftJustifiedStringAligner : StringAlignerBase
    {
        public LeftJustifiedStringAligner(string input) : base(input) {}

        protected override string Align(string[] lines)
        {
            return ProcessLines(lines, lineProcessor: line => line.TrimStart());
        }
    }
}
