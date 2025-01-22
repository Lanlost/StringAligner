using LandoWorks.StringAligner;

const String horizontalRule = "------------------------------------------------------------";

Console.WriteLine("Left Justified:" + Environment.NewLine + horizontalRule);

// Aligns all lines to the left.
Console.WriteLine(StringAligner.ToLeft(@"
        Hello World. 
     My name is Brent.
        This should be aligned to the left.
")); 


// Aligns all lines to the least indented line.
Console.WriteLine("Left-Most Aligned:" + Environment.NewLine + horizontalRule);

var test2 = StringAligner.ToLeftMost(@"
        Hello World.
     My name is Brent.
        This should be aligned to the least indented line.
");

Console.WriteLine(test2); 


// Aligns all lines to the mark on the first line.
Console.WriteLine("Mark-Aligned:" + Environment.NewLine + horizontalRule);

var markTest = StringAligner.ToMark('|', @"
        |Hello World.
         The BEST name is Brent.
 But Not Everyone Agrees!

         This should be the truth.
");

Console.WriteLine(markTest); 
Console.WriteLine("Mark Character was: " + markTest.MarkCharacter + Environment.NewLine);

// Test of using odd character for mark.
Console.WriteLine("Mark-Aligned #2:" + Environment.NewLine + horizontalRule);

var uniqueMarkChar = StringAligner.ToMark('', @"
        Hello World.
abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz
         abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz

         The lines here should start with 'h', 'j', 'a'.
");

Console.WriteLine(uniqueMarkChar); 
Console.WriteLine("Mark Character was: " + uniqueMarkChar.MarkCharacter + Environment.NewLine);

//////////////////////////////////////////////////////////////////////////////////////////////////

Console.WriteLine("Left Aligned:" + Environment.NewLine + horizontalRule);

Console.WriteLine(new LeftJustifiedStringAligner(@"
        Hello World. 
     My name is Brent.
        This should be aligned to the left.
"));


Console.WriteLine("Left-Most Aligned:" + Environment.NewLine + horizontalRule);

Console.WriteLine(new LeftmostStringAligner(@"
        Hello World.
     My name is Brent.
        This should be aligned to the least indented line.
"));


var markedAligned = new MarkedStringAligner('|', @"
        |Hello World.
         The BEST name is Brent.
 But Not Everyone Agrees!

         This should be the truth.
");
Console.WriteLine("Mark-Aligned:" + Environment.NewLine + horizontalRule);
Console.WriteLine(markedAligned);


// There are also extension methods for everything...
var extensionVersion = @"
        Hello World. 
     My name is Brent.
        This should be aligned to the left.".ToLeft();

Console.WriteLine("Extension Method Left Justified:" + Environment.NewLine + horizontalRule);
Console.WriteLine(extensionVersion);