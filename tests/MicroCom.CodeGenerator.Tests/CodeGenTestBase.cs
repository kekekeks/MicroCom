using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit;

namespace MicroCom.CodeGenerator.Tests
{
    public abstract class CodeGenTestBase
    {
        protected static string FindProjectRoot(string startDir)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "MicroCom.CodeGenerator.Tests.csproj")))
            {
                dir = dir.Parent;
            }
            if (dir == null)
                throw new DirectoryNotFoundException("Could not find project root containing MicroCom.CodeGenerator.Tests.csproj");
            return dir.FullName;
        }

        protected void RunAndAssertGenerator(bool expectParseError = false, string testClass = null, string testName = null, [CallerMemberName] string callerMemberName = null)
        {
            testClass ??= this.GetType().Name;
            testName ??= callerMemberName;
            var projectRoot = FindProjectRoot(AppContext.BaseDirectory);
            var root = Path.Combine(projectRoot, "cases", testClass);
            Directory.CreateDirectory(root);
            var idlPath = Path.Combine(root, $"{testName}.idl");
            var expectedCsPath = Path.Combine(root, $"{testName}.expected.cs");
            var expectedHPath = Path.Combine(root, $"{testName}.expected.h");
            var outCsPath = Path.Combine(root, $"{testName}.out.cs");
            var outHPath = Path.Combine(root, $"{testName}.out.h");

            var idl = File.ReadAllText(idlPath);

            if (expectParseError)
                Assert.ThrowsAny<Exception>(() => MicroComCodeGenerator.Parse(idl));
            else
            {
                var parsed = MicroCom.CodeGenerator.MicroComCodeGenerator.Parse(idl);
                if (expectParseError)
                    throw new Exception("Expected parse error, but parsing succeeded.");
                var generatedCs = parsed.GenerateCSharpInterop();
                var generatedH = parsed.GenerateCppHeader();

                File.WriteAllText(outCsPath, generatedCs);
                File.WriteAllText(outHPath, generatedH);

                var expectedCs = File.Exists(expectedCsPath) ? File.ReadAllText(expectedCsPath) : null;
                var expectedH = File.Exists(expectedHPath) ? File.ReadAllText(expectedHPath) : null;


                CodeGenTestHelpers.AssertGeneratorOutput(idl, expectedCs, expectedH);
            }
        }
    }
}
