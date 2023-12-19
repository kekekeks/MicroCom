using System;

namespace MicroCom.CodeGenerator
{
    class ParseException : Exception
    {
        public int Line { get; }
        public int Position { get; }
        public int CharacterOffsetFromStart { get; }

        public ParseException(string message, int line, int position, int characterOffsetFromStart) : base(message)
        {
            Line = line;
            Position = position;
            CharacterOffsetFromStart = characterOffsetFromStart;
        }

        public ParseException(string message, ref TokenParser parser) : this(message, parser.Line, parser.Position, parser.CharacterOffsetFromStart)
        {
        }
    }

    class CodeGenException : Exception
    {
        public CodeGenException(string message) : base(message)
        {
        }
    }
}
