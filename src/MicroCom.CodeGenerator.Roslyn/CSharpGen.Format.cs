using System;
using System.Collections.Generic;
using System.Linq;
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
            return SourcePreamble + unit.NormalizeWhitespace().ToFullString();
        }
    }
}