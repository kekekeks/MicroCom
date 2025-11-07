using System;
using Xunit;
using MicroCom.CodeGenerator;

namespace MicroCom.CodeGenerator.Tests
{
    public class TypeMappingTests : CodeGenTestBase
    {
        [Fact]
        public void Applies_Clr_Map_Directives()
        {
            RunAndAssertGenerator();
        }
    }
}
