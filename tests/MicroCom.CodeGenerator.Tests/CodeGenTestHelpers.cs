using System.Text.RegularExpressions;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public static class CodeGenTestHelpers
    {
        public static string NormalizeWhitespace(string input)
        {
            if (input == null) return null;
            return Regex.Replace(input, "\\s+", " ").Trim();
        }

        public static void AssertGeneratorOutput(string idl, string expectedCSharp, string expectedCpp = null)
        {
            var parsed = MicroComCodeGenerator.Parse(idl);
            var generatedCSharp = parsed.GenerateCSharpInterop();
            var normalizedGeneratedCSharp = NormalizeWhitespace(generatedCSharp);
            var normalizedExpectedCSharp = NormalizeWhitespace(expectedCSharp);
            Assert.Equal(normalizedExpectedCSharp, normalizedGeneratedCSharp);

            if (expectedCpp != null)
            {
                var generatedCpp = parsed.GenerateCppHeader();
                var normalizedGeneratedCpp = NormalizeWhitespace(generatedCpp);
                var normalizedExpectedCpp = NormalizeWhitespace(expectedCpp);
                Assert.Equal(normalizedExpectedCpp, normalizedGeneratedCpp);
            }
        }
    }
}

