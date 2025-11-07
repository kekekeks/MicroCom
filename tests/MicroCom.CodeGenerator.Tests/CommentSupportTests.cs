namespace MicroCom.CodeGenerator.Tests;

public class CommentSupportTests : CodeGenTestBase
{
    [Fact]
    public void XmlCommentSupport()
    {
        RunAndAssertGenerator();
    }
}