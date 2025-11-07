using System.Linq;
using MicroCom.CodeGenerator;
using Xunit;

namespace MicroCom.CodeGenerator.Tests
{
    public class AstTests
    {
        [Fact]
        public void ParsesStructComments()
        {
            var idl = @"// struct comment
struct Foo {
    // member comment
    int Bar;
}";
            var ast = AstParser.Parse(idl);
            var foo = ast.Structs.Single();
            Assert.Contains("struct comment", foo.Comments);
            var bar = foo.Single();
            Assert.Contains("member comment", bar.Comments);
        }

        [Fact]
        public void ParsesEnumComments()
        {
            var idl = @"// enum comment
enum E {
    // member1 comment
    A,
    // member2 comment
    B = 2
}";
            var ast = AstParser.Parse(idl);
            var e = ast.Enums.Single();
            Assert.Contains("enum comment", e.Comments);
            Assert.Contains("member1 comment", e[0].Comments);
            Assert.Contains("member2 comment", e[1].Comments);
        }
        [Fact]
        public void ParsesInterfaceComments()
        {
            var idl = @"// interface comment
interface IFoo {
    // method comment
    void Bar(// param comment
             int baz);
}";
            var ast = AstParser.Parse(idl);
            var iface = ast.Interfaces.Single();
            Assert.Contains("interface comment", iface.Comments);
            var bar = iface.Single();
            Assert.Contains("method comment", bar.Comments);
            var baz = bar.Single();
            Assert.Contains("param comment", baz.Comments);
        }

        [Fact]
        public void ParsesMultiLineComments()
        {
            var idl = @"/* struct comment */
struct Foo {
    /* member comment */
    int Bar;
}";
            var ast = AstParser.Parse(idl);
            var foo = ast.Structs.Single();
            Assert.Contains("struct comment", foo.Comments);
            var bar = foo.Single();
            Assert.Contains("member comment", bar.Comments);
        }

        [Fact]
        public void ParsesMixedComments()
        {
            var idl = @"// struct comment
/* another struct comment */
struct Foo {
    // member comment
    /* another member comment */
    int Bar;
}";
            var ast = AstParser.Parse(idl);
            var foo = ast.Structs.Single();
            Assert.Contains("struct comment", foo.Comments);
            Assert.Contains("another struct comment", foo.Comments);
            var bar = foo.Single();
            Assert.Contains("member comment", bar.Comments);
            Assert.Contains("another member comment", bar.Comments);
        }
    }
}
