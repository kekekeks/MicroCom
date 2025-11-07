using System.Collections.Generic;
using System.Xml.Linq;
using MicroCom.CodeGenerator;
using Xunit;

namespace MicroCom.CodeGenerator.Tests
{
    public class AstCommentHelperTests
    {
        [Fact]
        public void ParsesSimpleSummaryXmlComment()
        {
            var comments = new List<string> { "/// <summary>Hello world</summary>" };
            var xml = AstCommentHelper.ParseXmlComments(comments);
            Assert.NotNull(xml);
            Assert.Equal("Hello world", xml.Root.Element("summary")?.Value.Trim());
        }

        [Fact]
        public void ConvertsSummaryToDoxygen()
        {
            var comments = new List<string> { "/// <summary>Hello Doxygen</summary>" };
            var xml = AstCommentHelper.ParseXmlComments(comments);
            var doxy = AstCommentHelper.ToDoxygenComment(xml);
            Assert.Contains("Hello Doxygen", doxy);
            Assert.StartsWith("/**", doxy.Trim());
        }

        [Fact]
        public void ConvertsParamsAndReturnsToDoxygen()
        {
            var comments = new List<string>
            {
                "/// <summary>Method summary</summary>",
                "/// <param name=\"x\">X value</param>",
                "/// <param name=\"y\">Y value</param>",
                "/// <returns>Result</returns>"
            };
            var xml = AstCommentHelper.ParseXmlComments(comments);
            var doxy = AstCommentHelper.ToDoxygenComment(xml);
            Assert.Contains("Method summary", doxy);
            Assert.Contains("@param x X value", doxy);
            Assert.Contains("@param y Y value", doxy);
            Assert.Contains("@return Result", doxy);
        }

        [Fact]
        public void IgnoresNonXmlCommentLines()
        {
            var comments = new List<string>
            {
                "// Not XML",
                "/// <summary>XML summary</summary>",
                "Some other comment"
            };
            var xml = AstCommentHelper.ParseXmlComments(comments);
            Assert.NotNull(xml);
            Assert.Equal("XML summary", xml.Root.Element("summary")?.Value.Trim());
        }

        [Fact]
        public void ReturnsNullForNoXmlCommentLines()
        {
            var comments = new List<string> { "Just a comment", "// Another" };
            var xml = AstCommentHelper.ParseXmlComments(comments);
            Assert.Null(xml);
        }

        [Fact]
        public void HandlesMultiLineXmlComments()
        {
            var comments = new List<string>
            {
                "/// <summary>",
                "/// This is a multiline summary.",
                "/// </summary>",
                "/// <param name=\"foo\">Foo param</param>"
            };
            var xml = AstCommentHelper.ParseXmlComments(comments);
            Assert.NotNull(xml);
            var summary = xml.Root.Element("summary");
            Assert.Contains("multiline summary", summary?.Value);
            var doxy = AstCommentHelper.ToDoxygenComment(xml);
            Assert.Contains("multiline summary", doxy);
            Assert.Contains("@param foo Foo param", doxy);
        }
    }
}

