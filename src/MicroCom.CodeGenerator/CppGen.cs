using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MicroCom.CodeGenerator
{
    class CppGen
    {
        static string ConvertType(AstTypeNode type)
        {
            var name = type.Name;
            if (name == "byte")
                name = "unsigned char";
            else if(name == "uint")
                name = "unsigned int";

            type = type.Clone();
            type.Name = name;
            return type.Format();
        }

        static void AppendGenericParameters(StringBuilder sb, List<string> l)
        {
            if(l.Count == 0)
                return;
            sb.Append("template <");
            for(var c = 0; c < l.Count; c++)
            {
                sb.Append("typename ").Append(l[c]);
                if (c != l.Count - 1)
                    sb.Append(", ");
            }
            sb.AppendLine(">");
        }
        
        public static string GenerateCpp(AstIdlNode idl)
        {
            var sb = new StringBuilder();
            var preamble = idl.GetAttributeOrDefault("cpp-preamble");
            if (preamble != null)
                sb.AppendLine(preamble);

            foreach (var s in idl.Structs)
            {
                var doxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(s.Comments));
                if (!string.IsNullOrWhiteSpace(doxy))
                    sb.AppendLine(doxy);
                AppendGenericParameters(sb, s.GenericParameters);
                sb.AppendLine("struct " + s.Name + ";");
            }
            
            foreach (var s in idl.Interfaces)
            {
                var doxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(s.Comments));
                if (!string.IsNullOrWhiteSpace(doxy))
                    sb.AppendLine(doxy);
                sb.AppendLine("struct " + s.Name + ";");
            }

            foreach (var en in idl.Enums)
            {
                var doxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(en.Comments));
                if (!string.IsNullOrWhiteSpace(doxy))
                    sb.AppendLine(doxy);
                sb.Append("enum ");
                if (en.Attributes.Any(a => a.Name == "class-enum"))
                    sb.Append("class ");
                sb.AppendLine(en.Name).AppendLine("{");

                foreach (var m in en)
                {
                    var memberDoxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(m.Comments));
                    if (!string.IsNullOrWhiteSpace(memberDoxy))
                        sb.AppendLine("    " + memberDoxy.Replace("\n", "\n    ").TrimEnd());
                    sb.Append("    ").Append(m.Name);
                    if (m.Value != null)
                        sb.Append(" = ").Append(m.Value);
                    sb.AppendLine(",");
                }

                sb.AppendLine("};");
            }

            foreach (var s in idl.Structs)
            {
                var doxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(s.Comments));
                if (!string.IsNullOrWhiteSpace(doxy))
                    sb.AppendLine(doxy);
                AppendGenericParameters(sb, s.GenericParameters);
                sb.Append("struct ").AppendLine(s.Name).AppendLine("{");
                foreach (var m in s) 
                {
                    var memberDoxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(m.Comments));
                    if (!string.IsNullOrWhiteSpace(memberDoxy))
                        sb.AppendLine("    " + memberDoxy.Replace("\n", "\n    ").TrimEnd());
                    sb.Append("    ").Append(ConvertType(m.Type)).Append(" ").Append(m.Name).AppendLine(";");
                }
                sb.AppendLine("};");
            }

            foreach (var i in idl.Interfaces)
            {
                var doxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(i.Comments));
                if (!string.IsNullOrWhiteSpace(doxy))
                    sb.AppendLine(doxy);
                var guidString = i.GetAttribute("uuid");
                var guid = Guid.Parse(guidString).ToString().Replace("-", "");

                sb.Append("COMINTERFACE(").Append(i.Name).Append(", ")
                    .Append(guid.Substring(0, 8)).Append(", ")
                    .Append(guid.Substring(8, 4)).Append(", ")
                    .Append(guid.Substring(12, 4));
                for (var c = 0; c < 8; c++)
                {
                    sb.Append(", ").Append(guid.Substring(16 + c * 2, 2));
                }

                sb.Append(") : ");
                if (i.HasAttribute("cpp-virtual-inherits"))
                    sb.Append("virtual ");
                sb.AppendLine(i.Inherits ?? "IUnknown")
                    .AppendLine("{");

                foreach (var m in i)
                {
                    var memberDoxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(m.Comments));
                    if (!string.IsNullOrWhiteSpace(memberDoxy))
                        sb.AppendLine("    " + memberDoxy.Replace("\n", "\n    ").TrimEnd());
                    sb.Append("    ")
                        .Append("virtual ")
                        .Append(ConvertType(m.ReturnType))
                        .Append(" ").Append(m.Name).Append(" (");
                    if (m.Count == 0)
                        sb.AppendLine(") = 0;");
                    else
                    {
                        sb.AppendLine();
                        for (var c = 0; c < m.Count; c++)
                        {
                            var arg = m[c];
                            var argDoxy = AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(arg.Comments));
                            if (!string.IsNullOrWhiteSpace(argDoxy))
                                sb.AppendLine("        " + argDoxy.Replace("\n", "\n        ").TrimEnd());
                            if (arg.Attributes.Any(a => a.Name == "const"))
                                sb.Append("        const ");
                            else
                                sb.Append("        ");
                            sb.Append(ConvertType(arg.Type))
                                .Append(" ")
                                .Append(arg.Name);
                            if (c != m.Count - 1)
                                sb.Append(", ");
                            sb.AppendLine();
                        }

                        sb.AppendLine("    ) = 0;");
                    }
                }

                sb.AppendLine("};");
            }
            
            return sb.ToString();
        }
    }
}
