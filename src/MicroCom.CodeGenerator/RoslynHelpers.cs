using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace MicroCom.CodeGenerator
{
    static class RoslynHelpers
    {
        public static T WithXmlComments<T>(this T node, IAstNodeWithComments src) where T : SyntaxNode
        {
            if (src.Comments == null || src.Comments.Count == 0)
                return node;

            var triviaList = new List<SyntaxTrivia>();
            foreach (var comment in AstCommentHelper.ExtractXmlComments(src.Comments))
            {
                // Add as documentation comment (///)
                triviaList.Add(SyntaxFactory.Comment("/// " + comment));
            }

            return node.WithLeadingTrivia(triviaList);
        }
    }
}