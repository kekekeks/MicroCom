using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class CppCompileTests : CodeGenTestBase
    {
        private readonly ITestOutputHelper _output;

        public CppCompileTests(ITestOutputHelper output)
        {
            _output = output;
        }

        static string FindCompiler()
        {
            var fromEnv = Environment.GetEnvironmentVariable("CXX");
            if (!string.IsNullOrWhiteSpace(fromEnv))
                return fromEnv;
            foreach (var dir in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
            foreach (var name in new[] { "c++", "clang++", "g++" })
            {
                if (File.Exists(Path.Combine(dir, name)))
                    return Path.Combine(dir, name);
            }

            return null;
        }

        static (int exitCode, string output) Run(string fileName, params string[] args)
        {
            var psi = new ProcessStartInfo(fileName)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            foreach (var arg in args)
                psi.ArgumentList.Add(arg);
            using var process = Process.Start(psi)!;
            var stdout = process.StandardOutput.ReadToEndAsync();
            var stderr = process.StandardError.ReadToEndAsync();
            process.WaitForExit();
            return (process.ExitCode, stdout.Result + stderr.Result);
        }

        [Fact]
        public void Smart_Pointer_Wrappers_Compile_And_Dispatch_Through_Raw_Slots()
        {
            var compiler = FindCompiler();
            if (compiler == null || OperatingSystem.IsWindows())
            {
                _output.WriteLine("No Itanium ABI C++ compiler found, skipping");
                return;
            }

            var projectRoot = FindProjectRoot(AppContext.BaseDirectory);
            var tempDir = Path.Combine(Path.GetTempPath(), "microcom-cpp-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                var headers = new[]
                {
                    "Virtual_Inherits_Chains_Adapters", "Non_Virtual_Inherits_Flattens_Adapters",
                    "Wraps_Returns_And_Out_Params"
                };
                foreach (var name in headers)
                {
                    var idl = File.ReadAllText(Path.Combine(projectRoot, "cases", nameof(SmartPointerWrapperTests), name + ".idl"));
                    var header = MicroComCodeGenerator.Parse(idl)
                        .GenerateCppHeader(new CppGenOptions { SmartPointerWrappers = true });
                    File.WriteAllText(Path.Combine(tempDir, name + ".h"), header);
                }

                var cppDir = Path.Combine(projectRoot, "cpp");

                // Generated headers alone must be warning-free
                var headersOnly = Path.Combine(tempDir, "headers.cpp");
                File.WriteAllLines(headersOnly, new[] { "#include \"shim.h\"" }
                    .Concat(headers.Select(h => "#include \"" + h + ".h\"")));
                var (headersExit, headersOutput) = Run(compiler, "-std=c++17", "-Wall", "-Wextra", "-Werror",
                    "-fsyntax-only", "-I", tempDir, "-I", cppDir, headersOnly);
                Assert.True(headersExit == 0, headersOutput);

                // Test implementations use the ComObject : virtual IUnknown pattern, which triggers inaccessible-base
                var exe = Path.Combine(tempDir, "smoke");
                var (compileExit, compileOutput) = Run(compiler, "-std=c++17", "-Wall", "-Wextra", "-Werror", "-Wno-inaccessible-base",
                    "-I", tempDir, "-I", cppDir, "-o", exe, Path.Combine(cppDir, "smoke.cpp"));
                Assert.True(compileExit == 0, compileOutput);

                var (runExit, runOutput) = Run(exe);
                _output.WriteLine(runOutput);
                Assert.True(runExit == 0, runOutput);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
