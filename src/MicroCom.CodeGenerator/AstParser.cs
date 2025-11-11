using System;
using System.Collections.Generic;

namespace MicroCom.CodeGenerator
{
    class AstParser
    {
        public static AstIdlNode Parse(string source)
        {
            var parser = new TokenParser(source);
            var idl = new AstIdlNode { Attributes = ParseGlobalAttributes(ref parser) };
            while (!parser.Eof)
            {
                var attrs = ParseLocalAttributes(ref parser);
                if (parser.TryConsume(";"))
                    continue;
                if (parser.TryParseKeyword("enum"))
                {
                    var node = ParseEnum(attrs, ref parser);
                    idl.Enums.Add(node);
                }
                else if (parser.TryParseKeyword("struct"))
                {
                    var node = ParseStruct(attrs, ref parser);
                    idl.Structs.Add(node);
                }
                else if (parser.TryParseKeyword("interface"))
                {
                    var node = ParseInterface(attrs, ref parser);
                    idl.Interfaces.Add(node);
                }
                else
                    throw new ParseException("Unexpected character", ref parser);
            }
            return idl;
        }

        static AstAttributes ParseGlobalAttributes(ref TokenParser parser)
        {
            var rv = new AstAttributes();
            while (!parser.Eof)
            {
                parser.SkipWhitespace();
                if (parser.TryConsume('@'))
                {
                    var ident = parser.ParseIdentifier("-");
                    var value = parser.ReadToEol().Trim();
                    if (value == "@@")
                    {
                        parser.Advance(1);
                        value = "";
                        while (true)
                        {
                            var l = parser.ReadToEol();
                            if (l == "@@")
                                break;
                            else
                                value = value.Length == 0 ? l : (value + "\n" + l);
                            parser.Advance(1);
                        }

                    }
                    rv.Add(new AstAttributeNode(ident, value));
                }
                else
                    return rv;
            }

            return rv;
        }

        static AstAttributes ParseLocalAttributes(ref TokenParser parser)
        {
            var rv = new AstAttributes();
            while (parser.TryConsume("["))
            {
                while (!parser.TryConsume("]") && !parser.Eof)
                {
                    if (parser.TryConsume(','))
                        continue;

                    // Get identifier
                    var ident = parser.ParseIdentifier("-");

                    // No value, end of attribute list
                    if (parser.TryConsume(']'))
                    {
                        rv.Add(new AstAttributeNode(ident, null));
                        break;
                    }
                    // No value, next attribute
                    else if (parser.TryConsume(','))
                        rv.Add(new AstAttributeNode(ident, null));
                    // Has value
                    else if (parser.TryConsume('('))
                    {
                        var value = parser.ReadTo(')');
                        parser.Consume(')');
                        rv.Add(new AstAttributeNode(ident, value));
                    }
                    else
                        throw new ParseException("Unexpected character", ref parser);
                }

                if (parser.Eof)
                    throw new ParseException("Unexpected EOF", ref parser);
            }

            return rv;
        }

        static void EnsureOpenBracket(ref TokenParser parser)
        {
            if (!parser.TryConsume('{'))
                throw new ParseException("{ expected", ref parser);
        }

        static AstEnumNode ParseEnum(AstAttributes attrs, ref TokenParser parser)
        {
            var comments = parser.GetAndClearPrecedingComments();
            var name = parser.ParseIdentifier();
            EnsureOpenBracket(ref parser);
            var rv = new AstEnumNode { Name = name, Attributes = attrs, Comments = comments };
            while (!parser.TryConsume('}') && !parser.Eof)
            {
                var memberComments = parser.GetAndClearPrecedingComments();
                if (parser.TryConsume(','))
                    continue;
                var ident = parser.ParseIdentifier();
                // Automatic value
                if (parser.TryConsume(',') || parser.Peek == '}')
                {
                    var member = new AstEnumMemberNode(ident, null);
                    member.Comments = memberComments;
                    rv.Add(member);
                    continue;
                }
                if (!parser.TryConsume('='))
                    throw new ParseException("Unexpected character", ref parser);
                var value = parser.ReadToAny(",}").Trim();
                var memberNode = new AstEnumMemberNode(ident, value);
                memberNode.Comments = memberComments;
                rv.Add(memberNode);
                if (parser.Eof)
                    throw new ParseException("Unexpected EOF", ref parser);
            }
            return rv;
        }

        static AstTypeNode ParseType(ref TokenParser parser)
        {
            var ident = parser.ParseIdentifier();
            var t = new AstTypeNode { Name = ident };
            if (parser.TryConsume('<'))
            {
                do
                {
                    t.GenericArguments.Add(ParseType(ref parser));
                } while (parser.TryConsume(','));
                parser.SkipWhitespace();
                parser.Consume('>');
            }
            while (parser.TryConsume('*'))
                t.PointerLevel++;
            if (parser.TryConsume("&"))
                t.IsLink = true;
            return t;
        }

        static List<string> ParseGenericParameters( ref TokenParser parser)
        {
            var l = new List<string>();
            if (parser.TryConsume('<'))
            {
                do
                {
                    l.Add(parser.ParseIdentifier());
                } while (parser.TryConsume(','));
                if (!parser.TryConsume('>'))
                    throw new ParseException("> expected", ref parser);
            }
            return l;

        }

        static AstStructNode ParseStruct(AstAttributes attrs, ref TokenParser parser)
        {
            var comments = parser.GetAndClearPrecedingComments();
            var name = parser.ParseIdentifier();
            var genericParams = ParseGenericParameters(ref parser);
            EnsureOpenBracket(ref parser);
            var rv = new AstStructNode { Name = name, Attributes = attrs, GenericParameters = genericParams, Comments = comments };
            while (!parser.TryConsume('}'))
            {
                var memberComments = parser.GetAndClearPrecedingComments();
                var memberAttrs = ParseLocalAttributes(ref parser);
                var t = ParseType(ref parser);
                bool parsedAtLeastOneMember = false;
                while (!parser.TryConsume(';'))
                {
                    // Skip any ,
                    while (parser.TryConsume(',')) { }
                    var ident = parser.ParseIdentifier();
                    parsedAtLeastOneMember = true;
                    var memberNode = new AstStructMemberNode { Name = ident, Type = t, Attributes = memberAttrs, Comments = memberComments };
                    rv.Add(memberNode);
                }
                if (!parsedAtLeastOneMember)
                    throw new ParseException("Expected at least one enum member with declared type " + t, ref parser);
            }
            return rv;
        }

        static AstInterfaceNode ParseInterface(AstAttributes interfaceAttrs, ref TokenParser parser)
        {
            var comments = parser.GetAndClearPrecedingComments();
            var interfaceName = parser.ParseIdentifier();
            string inheritsFrom = null; 
            if (parser.TryConsume(":")) 
                inheritsFrom = parser.ParseIdentifier();
            EnsureOpenBracket(ref parser);
            var rv = new AstInterfaceNode
            {
                Name = interfaceName, Attributes = interfaceAttrs, Inherits = inheritsFrom, Comments = comments
            };
            while (!parser.TryConsume('}'))
            {
                var memberComments = parser.GetAndClearPrecedingComments();
                var memberAttrs = ParseLocalAttributes(ref parser);
                var returnType = ParseType(ref parser);
                var name = parser.ParseIdentifier();
                var member = new AstInterfaceMemberNode
                {
                    Name = name, ReturnType = returnType, Attributes = memberAttrs, Comments = memberComments
                };
                rv.Add(member);
                parser.Consume('(');
                while (true)
                {
                    var argComments = parser.GetAndClearPrecedingComments();
                    if (parser.TryConsume(')'))
                        break;
                    var argumentAttrs = ParseLocalAttributes(ref parser);
                    var type = ParseType(ref parser);
                    var argName = parser.ParseIdentifier();
                    member.Add(new AstInterfaceMemberArgumentNode
                    {
                        Name = argName, Type = type, Attributes = argumentAttrs, Comments = argComments
                    });
                    if (parser.TryConsume(')'))
                        break;
                    if (parser.TryConsume(','))
                        continue;
                    throw new ParseException("Unexpected character", ref parser);
                }
                parser.Consume(';');
            }
            return rv;
        }
    }
}
