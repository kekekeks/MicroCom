using System;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class InterfaceGenerationTests : CodeGenTestBase
    {
        [Fact]
        public void Generates_Interface_From_Idl()
        {
            RunAndAssertGenerator();
        }

        [Fact]
        public void NullableParameters()
        {
            RunAndAssertGenerator();
        }
    }
}