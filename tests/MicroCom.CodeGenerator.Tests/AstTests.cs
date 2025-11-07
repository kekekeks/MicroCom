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
            var idl = @"// struct comment\nstruct Foo {\n    // member comment\n    int Bar;\n}";
            var ast = AstParser.Parse(idl);
            var foo = ast.Structs.Single();
            Assert.Contains("struct comment", foo.Comments);
            var bar = foo.Single();
            Assert.Contains("member comment", bar.Comments);
        }

        [Fact]
        public void ParsesEnumComments()
        {
            var idl = @"// enum comment\nenum E {\n    // member1 comment\n    A,\n    // member2 comment\n    B = 2\n}";
            var ast = AstParser.Parse(idl);
            var e = ast.Enums.Single();
            Assert.Contains("enum comment", e.Comments);
            Assert.Contains("member1 comment", e[0].Comments);
            Assert.Contains("member2 comment", e[1].Comments);
        }

        [Fact]
        public void ParsesInterfaceComments()
        {
            var idl = @"// interface comment\ninterface IFoo {\n    // method comment\n    void Bar(// param comment\n             int baz);\n}";
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
            var idl = @"/* struct comment */\nstruct Foo {\n    /* member comment */\n    int Bar;\n}";
            var ast = AstParser.Parse(idl);
            var foo = ast.Structs.Single();
            Assert.Contains("struct comment", foo.Comments);
            var bar = foo.Single();
            Assert.Contains("member comment", bar.Comments);
        }

        [Fact]
        public void ParsesMixedComments()
        {
            var idl = @"// struct comment\n/* another struct comment */\nstruct Foo {\n    // member comment\n    /* another member comment */\n    int Bar;\n}";
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

