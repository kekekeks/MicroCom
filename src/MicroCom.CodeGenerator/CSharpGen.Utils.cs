using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace MicroCom.CodeGenerator
{
    partial class CSharpGen
    {

        CompilationUnitSyntax Unit()
            => CompilationUnit().WithUsings(List(new[]
                {
                    "System", "System.Text", "System.Collections", "System.Collections.Generic", RuntimeNamespace
                }
                .Concat(_extraUsings).Select(u => UsingDirective(IdentifierName(u)))));

        private const string SourcePreamble = @"#pragma warning disable 108
// ReSharper disable RedundantUsingDirective
// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedType.Local
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantCast
// ReSharper disable IdentifierTypo
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantUnsafeContext
// ReSharper disable RedundantBaseQualifier
// ReSharper disable EmptyStatement
// ReSharper disable RedundantAttributeParentheses
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
";
        
        SyntaxToken Semicolon() => Token(SyntaxKind.SemicolonToken);

        static VariableDeclarationSyntax DeclareVar(string type, string name,
            ExpressionSyntax? initializer = null)
            => VariableDeclaration(ParseTypeName(type),
                SingletonSeparatedList(VariableDeclarator(name)
                    .WithInitializer(initializer == null ? null : EqualsValueClause(initializer))));
        
        FieldDeclarationSyntax DeclareConstant(string type, string name, LiteralExpressionSyntax value)
            => FieldDeclaration(
                    VariableDeclaration(ParseTypeName(type),
                        SingletonSeparatedList(
                            VariableDeclarator(name).WithInitializer(EqualsValueClause(value))
                        ))
                ).WithSemicolonToken(Semicolon())
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ConstKeyword)));

        FieldDeclarationSyntax DeclareField(string type, string name, params SyntaxKind[] modifiers) =>
            DeclareField(type, name, null, modifiers);

        FieldDeclarationSyntax DeclareField(string type, string name, EqualsValueClauseSyntax initializer,
            params SyntaxKind[] modifiers) =>
            FieldDeclaration(
                    VariableDeclaration(ParseTypeName(type),
                        SingletonSeparatedList(
                            VariableDeclarator(name).WithInitializer(initializer))))
                .WithSemicolonToken(Semicolon())
                .WithModifiers(TokenList(modifiers.Select(x => Token(x))));

        bool IsPropertyRewriteCandidate(MethodDeclarationSyntax method)
        {

            return
                method.ReturnType.ToFullString() != "void"
                && method.Identifier.Text.StartsWith("Get")
                && method.ParameterList.Parameters.Count == 0;
        }

        TypeDeclarationSyntax RewriteMethodsToProperties<T>(T decl) where T : TypeDeclarationSyntax
        {
            var replace = new Dictionary<MethodDeclarationSyntax, PropertyDeclarationSyntax>();
            foreach (var method in decl.Members.OfType<MethodDeclarationSyntax>().ToList())
            {
                if (IsPropertyRewriteCandidate(method))
                {
                    var getter = AccessorDeclaration(SyntaxKind.GetAccessorDeclaration);
                    if (method.Body != null)
                        getter = getter.WithBody(method.Body);
                    else
                        getter = getter.WithSemicolonToken(Semicolon());

                    replace[method] = PropertyDeclaration(method.ReturnType,
                            method.Identifier.Text.Substring(3))
                        .WithModifiers(method.Modifiers).AddAccessorListAccessors(getter);

                }
            }

            return decl.ReplaceNodes(replace.Keys, (m, m2) => replace[m]);
        }

        bool IsInterface(string name)
        {
            if (name == "IUnknown")
                return true;
            return _idl.Interfaces.Any(i => i.Name == name);
        }

        private bool IsInterface(AstTypeNode type) => IsInterface(type.Name);

        static SyntaxTrivia IsNet5OrGreaterDirective =
            SyntaxTrivia(SyntaxKind.DisabledTextTrivia, "#if NET5_0_OR_GREATER\n");

        static SyntaxTrivia ElseDirective = SyntaxTrivia(SyntaxKind.DisabledTextTrivia, "\n#else\n");
        static SyntaxTrivia EndIfDirective = SyntaxTrivia(SyntaxKind.DisabledTextTrivia, "\n#endif");


    }
}
