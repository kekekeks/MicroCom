using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroCom.CodeGenerator
{
    class AstAttributeNode
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public AstAttributeNode(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"{Name} = {Value}";
        public AstAttributeNode Clone() => new AstAttributeNode(Name, Value);
    }

    class AstAttributes : List<AstAttributeNode>
    {
        public bool HasAttribute(string a) => this.Any(x => x.Name == a);
        
        public AstAttributes Clone()
        {
            var rv= new AstAttributes();
            rv.AddRange(this.Select(x => x.Clone()));
            return rv;
        }
    }

    interface IAstNodeWithComments
    {
        public List<string> Comments { get; set; }
    }
    
    interface IAstNodeWithAttributes : IAstNodeWithComments
    {
        AstAttributes Attributes { get; set; }
    }
    
    class AstEnumNode : List<AstEnumMemberNode>, IAstNodeWithAttributes
    {
        public AstAttributes Attributes { get; set; } = new AstAttributes();
        public string Name { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
        public override string ToString() => "Enum " + Name;

        public AstEnumNode Clone()
        {
            var rv = new AstEnumNode { Name = Name, Attributes = Attributes.Clone(), Comments = Comments.ToList() };
            rv.AddRange(this.Select(x => x.Clone()));
            return rv;
        }
    }

    class AstEnumMemberNode : IAstNodeWithComments
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<string> Comments { get; set; } = new List<string>();

        public AstEnumMemberNode(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public override string ToString() => $"Enum member {Name} = {Value}";
        public AstEnumMemberNode Clone() => new AstEnumMemberNode(Name, Value) { Comments = Comments.ToList() };
    }

    class AstStructNode : List<AstStructMemberNode>, IAstNodeWithAttributes
    {
        public AstAttributes Attributes { get; set; } = new AstAttributes();
        public List<string> GenericParameters { get; set; } = new List<string>();
        public string Name { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
        public override string ToString() => "Struct " + Name;
        
        public AstStructNode Clone()
        {
            var rv = new AstStructNode { Name = Name, Attributes = Attributes.Clone(), GenericParameters = GenericParameters.ToList(), Comments = Comments.ToList() };
            rv.AddRange(this.Select(x => x.Clone()));
            return rv;
        }
    }

    class AstTypeNode
    {
        public string Name { get; set; }
        public int PointerLevel { get; set; }
        public bool IsLink { get; set; }
        public List<AstTypeNode> GenericArguments { get; set; } = new List<AstTypeNode>();

        private void Format(StringBuilder sb)
        {
            sb.Append(Name);
            if (GenericArguments.Count > 0)
            {
                sb.Append('<');
                for (var c = 0; c < GenericArguments.Count; c++)
                {
                    var arg = GenericArguments[c];
                    arg.Format(sb);
                    if (c != GenericArguments.Count - 1)
                        sb.Append(", ");
                }

                sb.Append('>');
            }

            sb.Append('*', PointerLevel);
            if(IsLink)
                sb.Append('&');
        }
        
        public string Format()
        {
            var sb = new StringBuilder();
            Format(sb);
            return sb.ToString();
        }

        public override string ToString() => Format();

        public AstTypeNode Clone() => new AstTypeNode()
        {
            Name = Name,
            PointerLevel = PointerLevel,
            IsLink = IsLink,
            GenericArguments = GenericArguments.Select(a => a.Clone()).ToList()
        };
    }

    class AstStructMemberNode : IAstNodeWithAttributes
    {
        public string Name { get; set; }
        public AstTypeNode Type { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
        public override string ToString() => $"Struct member {Type.Format()} {Name}";
        public AstStructMemberNode Clone() => new AstStructMemberNode() { Name = Name, Type = Type.Clone(), Comments = Comments.ToList() };
        public AstAttributes Attributes { get; set; } = new AstAttributes();
    }

    class AstInterfaceNode : List<AstInterfaceMemberNode>, IAstNodeWithAttributes
    {
        public AstAttributes Attributes { get; set; } = new AstAttributes();
        public string Name { get; set; }
        public string Inherits { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
        
        public override string ToString()
        {
            if (Inherits == null)
                return Name;
            return $"Interface {Name} : {Inherits}";
        }
        public AstInterfaceNode Clone()
        {
            var rv = new AstInterfaceNode { Name = Name, Inherits = Inherits, Attributes = Attributes.Clone(), Comments = Comments.ToList() };
            rv.AddRange(this.Select(x => x.Clone()));
            return rv;
        }
    }

    class AstInterfaceMemberNode : List<AstInterfaceMemberArgumentNode>, IAstNodeWithAttributes
    {
        public string Name { get; set; }
        public AstTypeNode ReturnType { get; set; }
        public AstAttributes Attributes { get; set; } = new AstAttributes();
        public List<string> Comments { get; set; } = new List<string>();
        public AstInterfaceMemberNode Clone()
        {
            var rv = new AstInterfaceMemberNode()
            {
                Name = Name, Attributes = Attributes.Clone(), ReturnType = ReturnType, Comments = Comments.ToList()
            };
            rv.AddRange(this.Select(x => x.Clone()));
            return rv;
        }
        public override string ToString() =>
            $"Interface member {ReturnType.Format()} {Name} ({string.Join(", ", this.Select(x => x.Format()))})";
    }

    class AstInterfaceMemberArgumentNode : IAstNodeWithAttributes
    {
        public string Name { get; set; }
        public AstTypeNode Type { get; set; }
        public AstAttributes Attributes { get; set; } = new AstAttributes();
        public List<string> Comments { get; set; } = new List<string>();
        public  string Format() => $"{Type.Format()} {Name}";
        public override string ToString() => "Argument " + Format();
        public AstInterfaceMemberArgumentNode Clone() => new AstInterfaceMemberArgumentNode
        {
            Name = Name, Type = Type.Clone(), Attributes = Attributes.Clone(), Comments = Comments.ToList()
        };
    }

    static class AstExtensions
    {
        public static bool HasAttribute(this IAstNodeWithAttributes node, string s) => node.Attributes.HasAttribute(s);

        public static string GetAttribute(this IAstNodeWithAttributes node, string s)
        {
            var value = node.Attributes.FirstOrDefault(a => a.Name == s)?.Value;
            if (value == null)
                throw new CodeGenException("Expected attribute " + s + " for node " + node);
            return value;
        }
        
        public static string GetAttributeOrDefault(this IAstNodeWithAttributes node, string s) 
            => node.Attributes.FirstOrDefault(a => a.Name == s)?.Value;
    }

    class AstVisitor
    {
        protected virtual void VisitType(AstTypeNode type)
        {
        }
        
        protected virtual void VisitArgument(AstInterfaceMemberArgumentNode argument)
        {
            VisitType(argument.Type);
        }

        protected virtual void VisitInterfaceMember(AstInterfaceMemberNode member)
        {
            foreach(var a in member)
                VisitArgument(a);
            VisitType(member.ReturnType);
        }

        protected virtual void VisitInterface(AstInterfaceNode iface)
        {
            foreach(var m in iface)
                VisitInterfaceMember(m);
        }

        protected virtual void VisitStructMember(AstStructMemberNode member)
        {
            VisitType(member.Type);
        }

        protected virtual void VisitStruct(AstStructNode node)
        {
            foreach(var m in node)
                VisitStructMember(m);
        }
        
        public virtual void VisitAst(AstIdlNode ast)
        {
            foreach(var iface in ast.Interfaces)
                VisitInterface(iface);
            foreach (var s in ast.Structs)
                VisitStruct(s);
        }
        
        
    }

    class AstIdlNode : IAstNodeWithAttributes
    {
        public AstAttributes Attributes { get; set; } = new AstAttributes();
        public List<AstEnumNode> Enums { get; set; } = new List<AstEnumNode>();
        public List<AstStructNode> Structs { get; set; } = new List<AstStructNode>();
        public List<AstInterfaceNode> Interfaces { get; set; } = new List<AstInterfaceNode>();
        public List<string> Comments { get; set; } = new List<string>();

        public AstIdlNode Clone() => new AstIdlNode()
        {
            Attributes = Attributes.Clone(),
            Enums = Enums.Select(x => x.Clone()).ToList(),
            Structs = Structs.Select(x => x.Clone()).ToList(),
            Interfaces = Interfaces.Select(x => x.Clone()).ToList()
        };
    }
}
