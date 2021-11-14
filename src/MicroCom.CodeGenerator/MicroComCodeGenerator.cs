namespace MicroCom.CodeGenerator
{
    public class MicroComCodeGenerator
    {
        public static MicroComParsedFile Parse(string text)
        {
            return new MicroComParsedFile(AstParser.Parse(text));
        }
    }

    public class MicroComParsedFile
    {
        private readonly AstIdlNode _idl;
        internal AstIdlNode Idl => _idl;

        internal MicroComParsedFile(AstIdlNode idl)
        {
            _idl = idl;
        }

        public string GenerateCppHeader() => CppGen.GenerateCpp(_idl);

        public string GenerateCSharpInterop() => new CSharpGen(_idl).Generate();
    }
}