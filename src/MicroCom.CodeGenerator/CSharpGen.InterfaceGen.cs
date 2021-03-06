using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable CoVariantArrayConversion

// HERE BE DRAGONS

namespace MicroCom.CodeGenerator
{
    partial class CSharpGen
    {
        abstract class Arg
        {
            public string Name;
            public string NativeType;
            public AstAttributes Attributes { get; set; }
            public virtual StatementSyntax CreateFixed(StatementSyntax inner) => inner;

            public virtual void PreMarshal(List<StatementSyntax> body)
            {
            }

            public virtual void PreMarshalForReturn(List<StatementSyntax> body) =>
                throw new InvalidOperationException("Don't know how to use " + NativeType + " as HRESULT-return");

            public virtual ExpressionSyntax Value(bool isHresultReturn) => ParseExpression(Name);
            public abstract string ManagedType { get; }
            public virtual string ReturnManagedType => ManagedType;

            public virtual StatementSyntax[] ReturnMarshalResult() => new[] { ParseStatement("return " + Name + ";") };


            public virtual void BackPreMarshal(List<StatementSyntax> body)
            {
            }
            
            public virtual ExpressionSyntax BackMarshalValue() => ParseExpression(Name);
            public virtual ExpressionSyntax BackMarshalReturn(string resultVar) => ParseExpression(resultVar);
            
        }

        class InterfaceReturnArg : Arg
        {
            private readonly CSharpGen _gen;

            public InterfaceReturnArg(CSharpGen gen)
            {
                _gen = gen;
            }

            public string InterfaceType;
            public override ExpressionSyntax Value(bool isHresultReturn) => ParseExpression("&" + PName);
            public override string ManagedType => InterfaceType;

            private string PName => "__marshal_" + Name;

            public override void PreMarshalForReturn(List<StatementSyntax> body)
            {
                body.Add(ParseStatement("void* " + PName + " = null;"));
            }

            public override StatementSyntax[] ReturnMarshalResult() => new[]
            {
                ParseStatement($"return {_gen.RuntimeTypeName("MicroComRuntime")}.CreateProxyOrNullFor<" + InterfaceType + ">(" +
                               PName + ", true);")
            };

            public override ExpressionSyntax BackMarshalValue()
            {
                return ParseExpression("INVALID");
            }

            public override ExpressionSyntax BackMarshalReturn(string resultVar)
            {
                return ParseExpression(
                    _gen.RuntimeTypeName() + $".GetNativePointer({resultVar}, true)");
            }
        }

        class InterfaceArg : Arg
        {
            private readonly CSharpGen _gen;

            public InterfaceArg(CSharpGen gen)
            {
                _gen = gen;
            }

            public string InterfaceType;

            public override ExpressionSyntax Value(bool isHresultReturn) =>
                ParseExpression(_gen.RuntimeTypeName() + ".GetNativePointer(" + Name + ")");

            public override string ManagedType => InterfaceType;

            public override StatementSyntax[] ReturnMarshalResult() => new[]
            {
                ParseStatement($"return {_gen.RuntimeTypeName()}.CreateProxyOrNullFor<" + InterfaceType + ">(" +
                               Name + ", true);")
            };

            public override ExpressionSyntax BackMarshalValue()
            {
                return ParseExpression(_gen.RuntimeTypeName() + ".CreateProxyOrNullFor<" + InterfaceType + ">(" +
                                       Name + ", false)");
            }
            
            public override ExpressionSyntax BackMarshalReturn(string resultVar)
            {
                return ParseExpression(_gen.RuntimeTypeName() + $".GetNativePointer({resultVar}, true)");
            }
        }

        class BypassArg : Arg
        {
            public string Type { get; set; }
            public int PointerLevel;
            public override string ManagedType => Type + new string('*', PointerLevel);
            public override string ReturnManagedType => Type + new string('*', PointerLevel - 1);

            public override ExpressionSyntax Value(bool isHresultReturn)
            {
                if (isHresultReturn)
                    return ParseExpression("&" + Name);
                return base.Value(false);
            }

            public override void PreMarshalForReturn(List<StatementSyntax> body)
            {
                if (PointerLevel == 0)
                    base.PreMarshalForReturn(body);
                else
                    body.Add(ParseStatement(Type + new string('*', PointerLevel - 1) + " " + Name + "=default;"));
            }
        }

        class StringArg : Arg
        {
            private string BName => "__bytemarshal_" + Name;
            private string FName => "__fixedmarshal_" + Name;

            public override void PreMarshal(List<StatementSyntax> body)
            {
                body.Add(ParseStatement($"var {BName} = new byte[System.Text.Encoding.UTF8.GetByteCount({Name})+1];"));
                body.Add(ParseStatement($"System.Text.Encoding.UTF8.GetBytes({Name}, 0, {Name}.Length, {BName}, 0);"));
            }

            public override StatementSyntax CreateFixed(StatementSyntax inner)
            {
                return FixedStatement(DeclareVar("byte*", FName, ParseExpression(BName)), inner);
            }

            public override ExpressionSyntax Value(bool isHresultReturn) => ParseExpression(FName);
            public override string ManagedType => "string";

            public override StatementSyntax[] ReturnMarshalResult()
            {
                return new StatementSyntax[]
                {
                    ReturnStatement(ParseExpression(
                        $"({Name} == null ? null : System.Runtime.InteropServices.Marshal.PtrToStringAnsi(new IntPtr(" +
                        Name + ")))"))
                };
            }

            public override ExpressionSyntax BackMarshalValue()
            {
                return ParseExpression(
                    $"({Name} == null ? null : System.Runtime.InteropServices.Marshal.PtrToStringAnsi(new IntPtr(" + Name + ")))");
            }

            public override ExpressionSyntax BackMarshalReturn(string resultVar)
            {
                return ParseExpression(
                    $"({resultVar} == null ? null : (byte*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(" + resultVar + "))");
            }
        }

        class WideStringArg : Arg
        {
            private string FName => "__fixedmarshal_" + Name;

            public override StatementSyntax CreateFixed(StatementSyntax inner)
            {
                return FixedStatement(DeclareVar("void*", FName, ParseExpression(Name)), inner);
            }

            public override ExpressionSyntax Value(bool isHresultReturn) => ParseExpression(FName);
            public override string ManagedType => "string";

            public override StatementSyntax[] ReturnMarshalResult()
            {
                return new StatementSyntax[]
                {
                    ReturnStatement(ParseExpression(
                        $"({Name} == null ? null : System.Runtime.InteropServices.Marshal.PtrToStringUni(new IntPtr(" +
                        Name + ")))"))
                };
            }

            public override ExpressionSyntax BackMarshalValue()
            {
                return ParseExpression(
                    $"({Name} == null ? null : System.Runtime.InteropServices.Marshal.PtrToStringUni(new IntPtr(" + Name + ")))");
            }

            public override ExpressionSyntax BackMarshalReturn(string resultVar)
            {
                return ParseExpression(
                    $"({resultVar} == null ? null : (byte*)System.Runtime.InteropServices.Marshal.StringToHGlobalUni(" + resultVar + "))");
            }
        }

        string ConvertNativeType(string type)
        {
            if (type == "size_t")
                return "System.IntPtr";
            if (type == "HRESULT")
                return "int";
            return type;
        }

        Arg ConvertArg(AstInterfaceMemberArgumentNode node)
        {
            var arg = ConvertArg(node.Name, node.Type);
            arg.Attributes = node.Attributes.Clone();
            return arg;
        }
        
        Arg ConvertArg(string name, AstTypeNode type)
        {
            type = new AstTypeNode { Name = ConvertNativeType(type.Name), PointerLevel = type.PointerLevel };

            if (type.PointerLevel == 2)
            {
                if (IsInterface(type))
                    return new InterfaceReturnArg(this) { Name = name, InterfaceType = type.Name, NativeType = "void**" };
            }
            else if (type.PointerLevel == 1)
            {
                if (IsInterface(type))
                    return new InterfaceArg(this) { Name = name, InterfaceType = type.Name, NativeType = "void*" };
                if (type.Name == "char")
                    return new StringArg { Name = name, NativeType = "byte*" };
                if (type.Name == "char16_t" || type.Name == "WCHAR")
                    return new WideStringArg { Name = name, NativeType = "byte*" };
            }

            return new BypassArg
            {
                Name = name, Type = type.Name, PointerLevel = type.PointerLevel, NativeType = type.ToString()
            };
        }


        void GenerateInterfaceMember(AstInterfaceMemberNode member, ref InterfaceDeclarationSyntax iface,
            ref ClassDeclarationSyntax proxy, ref ClassDeclarationSyntax vtbl,
            List<StatementSyntax> vtblCtor, int num)
        {
            // Prepare method information
            if (member.Name == "GetRenderingDevice")
                Console.WriteLine();
            var args = member.Select(ConvertArg).ToList();
            var returnArg = ConvertArg("__result", member.ReturnType);
            bool isHresult = member.ReturnType.Name == "HRESULT";
            bool isHresultLastArgumentReturn = isHresult
                                               && args.Count > 0
                                               && (args.Last().Name == "ppv" 
                                                   || args.Last().Name == "retOut" 
                                                   || args.Last().Name == "ret"
                                                   || args.Last().Attributes.HasAttribute("out")
                                                   || args.Last().Attributes.HasAttribute("retval")
                                                   )
                                               && ((member.Last().Type.PointerLevel > 0
                                                    && !IsInterface(member.Last().Type))
                                                   || member.Last().Type.PointerLevel == 2);

            bool isVoidReturn = member.ReturnType.Name == "void" && member.ReturnType.PointerLevel == 0;


            // Generate method signature
            MethodDeclarationSyntax GenerateManagedSig(string returnType, string name,
                IEnumerable<(string n, string t)> sargs)
                => MethodDeclaration(ParseTypeName(returnType), name).WithParameterList(
                    ParameterList(
                        SeparatedList(sargs.Select(x => Parameter(Identifier(x.n)).WithType(ParseTypeName(x.t))))));

            var managedSig =
                isHresult ?
                    GenerateManagedSig(isHresultLastArgumentReturn ? args.Last().ReturnManagedType : "void",
                        member.Name,
                        (isHresultLastArgumentReturn ? args.SkipLast(1) : args).Select(a => (a.Name, a.ManagedType))) :
                    GenerateManagedSig(returnArg.ManagedType, member.Name, args.Select(a => (a.Name, a.ManagedType)));

            iface = iface.AddMembers(managedSig.WithSemicolonToken(Semicolon()));

            // Prepare args for marshaling
            var preMarshal = new List<StatementSyntax>();
            if (!isVoidReturn)
                preMarshal.Add(ParseStatement(returnArg.NativeType + " __result;"));

            for (var idx = 0; idx < args.Count; idx++)
            {
                if (isHresultLastArgumentReturn && idx == args.Count - 1)
                    args[idx].PreMarshalForReturn(preMarshal);
                else
                    args[idx].PreMarshal(preMarshal);
            }

            // Generate call expression

            var methodAddressExpression = ParseExpression("(*PPV)[base.VTableSize + " + num + "]");
            var methodPointerExpression = ParenthesizedExpression(CastExpression(GetFunctionPointerType(
                returnArg.NativeType,
                args.Select(x => x.NativeType).ToList(), true), methodAddressExpression));
            
            ExpressionSyntax callExpr = InvocationExpression(methodPointerExpression)
                .AddArgumentListArguments(Argument(ParseExpression("PPV")))
                .AddArgumentListArguments(args
                    .Select((a, i) => Argument(a.Value(isHresultLastArgumentReturn && i == args.Count - 1))).ToArray());

            if (!isVoidReturn)
                callExpr = CastExpression(ParseTypeName(returnArg.NativeType), callExpr);
            
            // Save call result if needed
            if (!isVoidReturn)
                callExpr = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, ParseExpression("__result"),
                    callExpr);


            // Wrap call into fixed() blocks
            StatementSyntax callStatement = ExpressionStatement(callExpr);
            foreach (var arg in args)
                callStatement = arg.CreateFixed(callStatement);

            // Build proxy body
            var proxyBody = Block()
                .AddStatements(preMarshal.ToArray())
                .AddStatements(callStatement);

            // Process return value
            if (!isVoidReturn)
            {
                if (isHresult)
                {
                    proxyBody = proxyBody.AddStatements(
                        ParseStatement(
                            $"if(__result != 0) throw new System.Runtime.InteropServices.COMException(\"{member.Name} failed\", __result);"));

                    if (isHresultLastArgumentReturn)
                        proxyBody = proxyBody.AddStatements(args.Last().ReturnMarshalResult());
                }
                else
                    proxyBody = proxyBody.AddStatements(returnArg.ReturnMarshalResult());
            }

            // Add the proxy method
            proxy = proxy.AddMembers(managedSig.AddModifiers(SyntaxKind.PublicKeyword)
                .WithBody(proxyBody));

            
            // Generate VTable method
            var shadowDelegate = DelegateDeclaration(ParseTypeName(returnArg.NativeType), member.Name + "Delegate")
                .AddParameterListParameters(Parameter(Identifier("@this")).WithType(ParseTypeName("void*")))
                .AddParameterListParameters(args.Select(x =>
                    Parameter(Identifier(x.Name)).WithType(ParseTypeName(x.NativeType))).ToArray())
                .AddAttribute("System.Runtime.InteropServices.UnmanagedFunctionPointer",
                    "System.Runtime.InteropServices.CallingConvention.StdCall");

            var shadowMethod = MethodDeclaration(shadowDelegate.ReturnType, member.Name)
                .WithParameterList(shadowDelegate.ParameterList)
                .AddModifiers(Token(SyntaxKind.StaticKeyword));
            var attr = AttributeList(SingletonSeparatedList(
                    Attribute(ParseName("System.Runtime.InteropServices.UnmanagedCallersOnly"))
                        .WithArgumentList(ParseAttributeArgumentList(
                            "(CallConvs = new []{typeof(System.Runtime.CompilerServices.CallConvStdcall)})"))))
                .WithLeadingTrivia(SyntaxTriviaList.Create(IsNet5OrGreaterDirective))
                .WithTrailingTrivia(SyntaxTriviaList.Create(EndIfDirective));
                
            shadowMethod = shadowMethod.AddAttributeLists(attr);
            

            var backPreMarshal = new List<StatementSyntax>();
            foreach (var arg in args)
                arg.BackPreMarshal(backPreMarshal);

            backPreMarshal.Add(
                ParseStatement($"__target = ({iface.Identifier.Text}){RuntimeTypeName()}.GetObjectFromCcw(new IntPtr(@this));"));

            var isBackVoidReturn = isVoidReturn || (isHresult && !isHresultLastArgumentReturn);

            StatementSyntax backCallStatement;

            var backCallExpr =
                IsPropertyRewriteCandidate(managedSig) ?
                    ParseExpression("__target." + member.Name.Substring(3)) :
                    InvocationExpression(ParseExpression("__target." + member.Name))
                        .WithArgumentList(ArgumentList(SeparatedList(
                            (isHresultLastArgumentReturn ? args.SkipLast(1) : args)
                            .Select(a =>
                                Argument(a.BackMarshalValue())))));

            if (isBackVoidReturn)
                backCallStatement = ExpressionStatement(backCallExpr);
            else
            {
                backCallStatement = LocalDeclarationStatement(DeclareVar("var", "__result", backCallExpr));
                if (isHresultLastArgumentReturn)
                {
                    backCallStatement = Block(backCallStatement,
                        ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                            ParseExpression("*" + args.Last().Name),
                            args.Last().BackMarshalReturn("__result")
                        )));

                }
                else
                    backCallStatement = Block(backCallStatement,
                        ReturnStatement(returnArg.BackMarshalReturn("__result")));
            }

            BlockSyntax backBodyBlock = Block().AddStatements(backPreMarshal.ToArray()).AddStatements(backCallStatement);


            var exceptions = new List<CatchClauseSyntax>()
            {
                CatchClause(
                    CatchDeclaration(ParseTypeName("System.Exception"), Identifier("__exception__")), null,
                    Block(
                        ParseStatement(
                            RuntimeTypeName() + ".UnhandledException(__target, __exception__);"),
                        isHresult ? ParseStatement("return unchecked((int)0x80004005u);")
                        : isVoidReturn ? EmptyStatement() : ParseStatement("return default;")
                    ))
            };
            
            if (isHresult)
                exceptions.Insert(0, CatchClause(
                    CatchDeclaration(ParseTypeName("System.Runtime.InteropServices.COMException"),
                        Identifier("__com_exception__")),
                    null, Block(ParseStatement("return __com_exception__.ErrorCode;"))));

            backBodyBlock = Block(
                TryStatement(
                        List(exceptions))
                    .WithBlock(Block(backBodyBlock))
            );
            if (isHresult)
                backBodyBlock = backBodyBlock.AddStatements(ParseStatement("return 0;"));
            

            backBodyBlock = Block()
                .AddStatements(ParseStatement($"{iface.Identifier.Text} __target = null;"))
                .AddStatements(backBodyBlock.Statements.ToArray());

            shadowMethod = shadowMethod.WithBody(backBodyBlock);

            vtbl = vtbl.AddMembers(shadowDelegate);
            vtbl = vtbl.AddMembers(shadowMethod);

            vtblCtor.Add(ExpressionStatement(InvocationExpression(
                    ParseExpression("base.AddMethod"),
                    ArgumentList(SingletonSeparatedList(Argument(
                        CastExpression(GetFunctionPointerType(
                                returnArg.NativeType,
                                args.Select(x => x.NativeType).ToList(), false),
                            ParseExpression("&" + shadowMethod.Identifier.Text))
                    )))
                ))
                .WithLeadingTrivia(IsNet5OrGreaterDirective)
                .WithTrailingTrivia(ElseDirective));
            
            vtblCtor.Add(ParseStatement("base.AddMethod((" + shadowDelegate.Identifier.Text + ")" +
                                        shadowMethod.Identifier.Text + ");").WithTrailingTrivia(EndIfDirective));




        }

        FunctionPointerTypeSyntax GetFunctionPointerType(string returnType, List<string> args, bool convertToVoidPtr)
        {
            string ConvertType(string t) => t.EndsWith("*") ? "void*" : t;
            returnType = ConvertType(returnType);

            if (convertToVoidPtr)
                args = args.Select(ConvertType).ToList();
            else
                args = args.ToList();
            
            args.Insert(0, "void*");
            args.Add(returnType);
            
            return FunctionPointerType(FunctionPointerCallingConvention(
                    SyntaxFactory.
                    Token(SyntaxKind.UnmanagedKeyword),
                    FunctionPointerUnmanagedCallingConventionList(
                        SingletonSeparatedList(FunctionPointerUnmanagedCallingConvention(Identifier("Stdcall"))))),
                FunctionPointerParameterList(SeparatedList(
                    args.Select(a => FunctionPointerParameter(ParseTypeName(a)))
                )));
        }


        void GenerateInterface(ref NamespaceDeclarationSyntax ns, ref NamespaceDeclarationSyntax implNs,
            AstInterfaceNode iface)
        {
            var guidString = iface.GetAttribute("uuid");
            var inheritsUnknown = iface.Inherits == null || iface.Inherits == "IUnknown";

            var ifaceDec = InterfaceDeclaration(iface.Name)
                .WithBaseType(inheritsUnknown ? RuntimeTypeName("IUnknown") : iface.Inherits)
                .AddModifiers(Token(_visibility), Token(SyntaxKind.UnsafeKeyword), Token(SyntaxKind.PartialKeyword));

            var proxyClassName = "__MicroCom" + iface.Name + "Proxy";
            var proxy = ClassDeclaration(proxyClassName)
                .AddModifiers(Token(_visibility), Token(SyntaxKind.UnsafeKeyword), Token(SyntaxKind.PartialKeyword))
                .WithBaseType(inheritsUnknown ?
                    RuntimeTypeName("MicroComProxyBase") :
                    ("__MicroCom" + iface.Inherits + "Proxy"))
                .AddBaseListTypes(SimpleBaseType(ParseTypeName(iface.Name)));


            // Generate vtable
            var vtbl = ClassDeclaration("__MicroCom" + iface.Name + "VTable")
                .AddModifiers(Token(SyntaxKind.UnsafeKeyword));

            vtbl = vtbl.WithBaseType(inheritsUnknown ?
                RuntimeTypeName("MicroComVtblBase") :
                "__MicroCom" + iface.Inherits + "VTable");
            
            var vtblCtor = new List<StatementSyntax>();
            for (var idx = 0; idx < iface.Count; idx++)
                GenerateInterfaceMember(iface[idx], ref ifaceDec, ref proxy, ref vtbl, vtblCtor, idx);

            vtbl = vtbl.AddMembers(
                    ConstructorDeclaration(vtbl.Identifier.Text)
                        .AddModifiers(Token(SyntaxKind.ProtectedKeyword))
                        .WithBody(Block(vtblCtor))
                )
                .AddMembers(MethodDeclaration(ParseTypeName("void"), "__MicroComModuleInit")
                    .AddModifiers(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword))
                    .AddAttribute("System.Runtime.CompilerServices.ModuleInitializer")
                    .WithExpressionBody(ArrowExpressionClause(
                        ParseExpression(RuntimeTypeName() + ".RegisterVTable(typeof(" +
                                        iface.Name + "), new " + vtbl.Identifier.Text + "().CreateVTable())")))
                    .WithSemicolonToken(Semicolon()));
                
            
            // Finalize proxy code
            proxy = proxy.AddMembers(
                    MethodDeclaration(ParseTypeName("void"), "__MicroComModuleInit")
                        .AddModifiers(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword))
                        .AddAttribute("System.Runtime.CompilerServices.ModuleInitializer")
                        .WithBody(Block(
                            ParseStatement(RuntimeTypeName() + ".Register(typeof(" +
                                           iface.Name + "), new Guid(\"" + guidString + "\"), (p, owns) => new " +
                                           proxyClassName + "(p, owns));")
                        )))
                .AddMembers(ParseMemberDeclaration("protected " + proxyClassName +
                                                   "(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle) {}"))
                .AddMembers(ParseMemberDeclaration("protected override int VTableSize => base.VTableSize + " +
                                                   iface.Count + ";"));
            
            ns = ns.AddMembers(RewriteMethodsToProperties(ifaceDec));
            implNs = implNs.AddMembers(RewriteMethodsToProperties(proxy), RewriteMethodsToProperties(vtbl));
        }
    }
}
