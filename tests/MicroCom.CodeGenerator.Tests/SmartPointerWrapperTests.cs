using System;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class SmartPointerWrapperTests : CodeGenTestBase
    {
        static CppGenOptions Options => new CppGenOptions { SmartPointerWrappers = true };

        [Fact]
        public void Wraps_Returns_And_Out_Params()
        {
            RunAndAssertGenerator(cppOptions: Options);
        }

        [Fact]
        public void Custom_Smart_Pointer_Type()
        {
            RunAndAssertGenerator(cppOptions: new CppGenOptions { SmartPointerWrappers = true, SmartPointerType = "MyPtr" });
        }

        [Fact]
        public void Virtual_Inherits_Chains_Adapters()
        {
            RunAndAssertGenerator(cppOptions: Options);
        }

        [Fact]
        public void Non_Virtual_Inherits_Flattens_Adapters()
        {
            RunAndAssertGenerator(cppOptions: Options);
        }

        [Fact]
        public void Call_Preamble_And_Retention()
        {
            RunAndAssertGenerator(cppOptions: Options);
        }

        [Fact]
        public void Unknown_Base_Fails()
        {
            var parsed = MicroComCodeGenerator.Parse(@"@clr-namespace Test
@clr-access internal
[uuid(0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f41)]
interface IFoo : IExternal
{
    void DoWork();
}
");
            var e = Assert.ThrowsAny<Exception>(() => parsed.GenerateCppHeader(Options));
            Assert.Contains("IExternal", e.Message);
        }
    }
}
