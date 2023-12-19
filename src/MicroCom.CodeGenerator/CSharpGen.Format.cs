using System.Composition.Hosting;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Host.Mef;
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
        string Format(CompilationUnitSyntax unit)
        {
            var cw = new AdhocWorkspace();
            return SourcePreamble +
                Microsoft.CodeAnalysis.Formatting.Formatter.Format(unit.NormalizeWhitespace(), cw, cw.Options
                    .WithChangedOption(CSharpFormattingOptions.NewLineForMembersInObjectInit, true)
                    .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers,
                        true)
                    .WithChangedOption(CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, true)
                    .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true)

                ).ToFullString();
        }
    }
}