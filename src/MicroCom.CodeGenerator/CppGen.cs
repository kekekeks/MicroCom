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
        
        const string ImplPrefix = "impl_";

        const string ImplAdapterPreamble = @"template<class T> struct MicroComImpl;
#ifndef MICROCOM_IMPL_BASE_DEFINED
#define MICROCOM_IMPL_BASE_DEFINED
// Non-virtual primary base for adapters, so they never take over the primary base of an interface (Itanium ABI)
template<class T> struct MicroComImplBase
{
    virtual ~MicroComImplBase() = default;
};
#endif
";

        class SmartPointerContext
        {
            private readonly AstIdlNode _idl;
            private readonly string _smartPointer;

            public SmartPointerContext(AstIdlNode idl, string smartPointer)
            {
                _idl = idl;
                _smartPointer = string.IsNullOrWhiteSpace(smartPointer) ? "ComPtr" : smartPointer;
            }

            bool IsInterface(AstTypeNode type) => !type.IsLink && type.GenericArguments.Count == 0
                && (type.Name == "IUnknown" || _idl.Interfaces.Any(i => i.Name == type.Name));

            public bool IsWrappedReturn(AstTypeNode type) => type.PointerLevel == 1 && IsInterface(type);

            public bool IsWrappedArg(AstInterfaceMemberArgumentNode arg) => arg.Type.PointerLevel == 2 && IsInterface(arg.Type);

            public bool IsWrapped(AstInterfaceMemberNode member) => IsWrappedReturn(member.ReturnType) || member.Any(IsWrappedArg);

            public string SmartType(string interfaceName) => _smartPointer + "<" + interfaceName + ">";

            public string WrapperReturnType(AstInterfaceMemberNode member) => IsWrappedReturn(member.ReturnType)
                ? SmartType(member.ReturnType.Name)
                : ConvertType(member.ReturnType);

            public string WrapperArgType(AstInterfaceMemberArgumentNode arg) => IsWrappedArg(arg)
                ? SmartType(arg.Type.Name) + "&"
                : ConvertArgType(arg);
        }

        static string ConvertArgType(AstInterfaceMemberArgumentNode arg) =>
            (arg.Attributes.Any(a => a.Name == "const") ? "const " : "") + ConvertType(arg.Type);

        static bool IsVoid(AstTypeNode type) => type.Name == "void" && type.PointerLevel == 0;

        static void AppendDoxy(StringBuilder sb, string doxy, string indent)
        {
            if (!string.IsNullOrWhiteSpace(doxy))
                sb.AppendLine(indent + doxy.Replace("\n", "\n" + indent).TrimEnd());
        }

        static void AppendSignature(StringBuilder sb, string indent, string prefix, string returnType, string name,
            AstInterfaceMemberNode member, Func<AstInterfaceMemberArgumentNode, string> argType, bool argDocs, string suffix)
        {
            sb.Append(indent).Append(prefix).Append(returnType).Append(" ").Append(name).Append(" (");
            if (member.Count == 0)
            {
                sb.Append(")").AppendLine(suffix);
                return;
            }

            sb.AppendLine();
            for (var c = 0; c < member.Count; c++)
            {
                var arg = member[c];
                if (argDocs)
                    AppendDoxy(sb, AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(arg.Comments)),
                        indent + "    ");
                sb.Append(indent).Append("    ").Append(argType(arg)).Append(" ").Append(arg.Name);
                if (c != member.Count - 1)
                    sb.Append(", ");
                sb.AppendLine();
            }

            sb.Append(indent).Append(")").AppendLine(suffix);
        }

        static void AppendWrapperDefinitions(StringBuilder sb, AstIdlNode idl, SmartPointerContext ctx)
        {
            foreach (var i in idl.Interfaces)
            foreach (var m in i.Where(ctx.IsWrapped))
            {
                AppendSignature(sb, "", "inline ", ctx.WrapperReturnType(m), i.Name + "::" + m.Name, m,
                    ctx.WrapperArgType, false, "");
                var call = ImplPrefix + m.Name + "(" + string.Join(", ",
                    m.Select(a => ctx.IsWrappedArg(a) ? a.Name + "._microcom_out()" : a.Name)) + ")";
                sb.AppendLine("{");
                if (ctx.IsWrappedReturn(m.ReturnType))
                {
                    sb.Append("    ").Append(ctx.SmartType(m.ReturnType.Name)).AppendLine(" __microcom_rv;");
                    sb.Append("    __microcom_rv._microcom_attach(").Append(call).AppendLine(");");
                    sb.AppendLine("    return __microcom_rv;");
                }
                else if (IsVoid(m.ReturnType))
                    sb.Append("    ").Append(call).AppendLine(";");
                else
                    sb.Append("    return ").Append(call).AppendLine(";");
                sb.AppendLine("}");
            }
        }

        static void AppendImplAdapters(StringBuilder sb, AstIdlNode idl, SmartPointerContext ctx)
        {
            var virtualBases = new HashSet<string>(idl.Interfaces
                .Where(i => i.HasAttribute("cpp-virtual-inherits"))
                .Select(i => i.Inherits ?? "IUnknown"));

            foreach (var i in idl.Interfaces)
            {
                var bases = new List<string>
                {
                    "public MicroComImplBase<" + i.Name + ">",
                    (virtualBases.Contains(i.Name) ? "public virtual " : "public ") + i.Name
                };

                // Non-virtual bases are separate subobjects and get flattened into this adapter,
                // virtual bases are shared and implemented by their own adapter
                var members = new List<AstInterfaceMemberNode>();
                for (var current = i;;)
                {
                    members.AddRange(current.Where(ctx.IsWrapped));
                    if (current.Inherits == null || current.Inherits == "IUnknown")
                        break;
                    var baseInterface = idl.Interfaces.FirstOrDefault(x => x.Name == current.Inherits)
                                        ?? throw new CodeGenException("Unable to generate MicroComImpl<" + i.Name +
                                                                      ">: base interface " + current.Inherits +
                                                                      " is not defined in this IDL file");
                    if (current.HasAttribute("cpp-virtual-inherits"))
                    {
                        bases.Add("public virtual MicroComImpl<" + baseInterface.Name + ">");
                        break;
                    }

                    current = baseInterface;
                }

                sb.AppendLine("template<>");
                sb.Append("struct MicroComImpl<").Append(i.Name).Append("> : ").AppendLine(string.Join(", ", bases));
                sb.AppendLine("{");
                foreach (var m in members)
                {
                    AppendDoxy(sb, AstCommentHelper.ToDoxygenComment(AstCommentHelper.ParseXmlComments(m.Comments)), "    ");
                    AppendSignature(sb, "    ", "virtual ", ctx.WrapperReturnType(m), m.Name, m, ctx.WrapperArgType,
                        true, " = 0;");
                }

                foreach (var m in members)
                    AppendThunk(sb, m, ctx);
                sb.AppendLine("};");
            }
        }

        static void AppendThunk(StringBuilder sb, AstInterfaceMemberNode m, SmartPointerContext ctx)
        {
            AppendSignature(sb, "    ", "", ConvertType(m.ReturnType), ImplPrefix + m.Name, m, ConvertArgType, false,
                " final");
            sb.AppendLine("    {");
            var outs = m.Where(ctx.IsWrappedArg).ToList();
            foreach (var o in outs)
                sb.Append("        ").Append(ctx.SmartType(o.Type.Name)).Append(" __microcom_").Append(o.Name).AppendLine(";");

            var call = m.Name + "(" + string.Join(", ",
                m.Select(a => ctx.IsWrappedArg(a) ? "__microcom_" + a.Name : a.Name)) + ")";
            if (ctx.IsWrappedReturn(m.ReturnType))
                call += "._microcom_detach()";

            var isVoid = IsVoid(m.ReturnType);
            if (outs.Count == 0)
                sb.Append("        ").Append(isVoid ? "" : "return ").Append(call).AppendLine(";");
            else
            {
                sb.Append("        ").Append(isVoid ? "" : "auto __microcom_r = ").Append(call).AppendLine(";");
                foreach (var o in outs)
                    sb.Append("        if (").Append(o.Name).Append(") *").Append(o.Name).Append(" = __microcom_")
                        .Append(o.Name).AppendLine("._microcom_detach();");
                if (!isVoid)
                    sb.AppendLine("        return __microcom_r;");
            }

            sb.AppendLine("    }");
        }

        public static string GenerateCpp(AstIdlNode idl, CppGenOptions options = null)
        {
            var ctx = options?.SmartPointerWrappers == true ? new SmartPointerContext(idl, options.SmartPointerType) : null;
            var sb = new StringBuilder();
            var preamble = idl.GetAttributeOrDefault("cpp-preamble");
            if (preamble != null)
                sb.AppendLine(preamble);

            if (ctx != null)
                sb.Append(ImplAdapterPreamble);

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
                    var wrapped = ctx?.IsWrapped(m) == true;
                    AppendDoxy(sb, memberDoxy, "    ");
                    AppendSignature(sb, "    ", "virtual ", ConvertType(m.ReturnType),
                        wrapped ? ImplPrefix + m.Name : m.Name, m, ConvertArgType, true, " = 0;");
                    if (wrapped)
                    {
                        AppendDoxy(sb, memberDoxy, "    ");
                        AppendSignature(sb, "    ", "", ctx.WrapperReturnType(m), m.Name, m, ctx.WrapperArgType, true, ";");
                    }
                }

                sb.AppendLine("};");
            }

            if (ctx != null)
            {
                AppendWrapperDefinitions(sb, idl, ctx);
                AppendImplAdapters(sb, idl, ctx);
            }
            
            return sb.ToString();
        }
    }
}
