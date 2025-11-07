using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MicroCom.CodeGenerator
{
    static class AstCommentHelper
    {
        /// <summary>
        /// Parses XML-style comments from a list of comment lines.
        /// Lines starting with '/' are treated as XML comment lines.
        /// </summary>
        /// <param name="comments">List of comment lines</param>
        /// <returns>XDocument containing the parsed XML comments, or null if none found or invalid.</returns>
        public static XDocument ParseXmlComments(List<string> comments)
        {
            if (comments == null || comments.Count == 0)
                return null;
            // Filter lines starting with '/'
            var xmlLines = comments.Where(l => l.TrimStart().StartsWith("/")).ToList();
            if (xmlLines.Count == 0)
                return null;
            // Remove leading '/' and whitespace
            var xmlContent = string.Join("\n", xmlLines.Select(l => l.TrimStart().TrimStart('/').Trim()));
            // Wrap in a root node
            var wrapped = "<root>" + xmlContent + "</root>";
            var parsed = XDocument.Parse(wrapped);
            return parsed.Root.HasElements ? parsed : null;

        }

        /// <summary>
        /// Converts XML comments (XDocument) into Doxygen comment format.
        /// </summary>
        /// <param name="xmlDoc">Parsed XML comments</param>
        /// <returns>Doxygen comment string or null if input is null/empty</returns>
        public static string ToDoxygenComment(XDocument xmlDoc)
        {
            if (xmlDoc == null || xmlDoc.Root == null)
                return null;
            var lines = new List<string>();
            // Process <summary>
            var summary = xmlDoc.Root.Element("summary");
            if (summary != null)
                lines.Add(summary.Value.Trim());
            // Process <param>
            foreach (var param in xmlDoc.Root.Elements("param"))
            {
                var name = param.Attribute("name")?.Value;
                var text = param.Value.Trim();
                if (!string.IsNullOrEmpty(name))
                    lines.Add($"@param {name} {text}");
            }
            // Process <returns>
            var returns = xmlDoc.Root.Element("returns");
            if (returns != null)
                lines.Add($"@return {returns.Value.Trim()}");
            // Process <remarks>
            var remarks = xmlDoc.Root.Element("remarks");
            if (remarks != null)
                lines.Add($"@remarks {remarks.Value.Trim()}");
            // Process any other tags as plain text
            foreach (var node in xmlDoc.Root.Elements())
            {
                if (node.Name != "summary" && node.Name != "param" && node.Name != "returns" && node.Name != "remarks")
                    lines.Add(node.Value.Trim());
            }
            // Format as Doxygen block comment
            if (lines.Count == 0)
                return null;
            var doxygen = "/**\n" + string.Join("\n", lines.Select(l => " * " + l)) + "\n */";
            return doxygen;
        }
    }
}
