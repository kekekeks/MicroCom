using System;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class EnumGenerationTests : CodeGenTestBase
    {
        [Fact]
        public void Generates_Enum_From_Idl()
        {
            RunAndAssertGenerator();
        }
    }
}
