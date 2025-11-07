using System;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class ErrorHandlingTests
    {
        [Fact]
        public void Throws_On_Invalid_Idl()
        {
            var invalidIdl = @"@clr-namespace Test
@clr-access public
struct BrokenStruct
{
    int a; // Missing closing brace
";
            Assert.ThrowsAny<Exception>(() =>
            {
                var parsed = MicroComCodeGenerator.Parse(invalidIdl);
                var generated = parsed.GenerateCSharpInterop();
            });
        }
    }
}

