using System;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class StructGenerationTests : CodeGenTestBase
    {
        [Fact]
        public void Generates_Structs_With_Generics_From_Idl()
        {
            RunAndAssertGenerator();
        }
    }
}
