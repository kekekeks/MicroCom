namespace MicroCom.CodeGenerator
{
    public class MicroComCodeGenerator
    {
        public static MicroComParsedFile Parse(string text)
        {
            return new MicroComParsedFile(AstParser.Parse(text));
        }
    }

    public class CppGenOptions
    {
        /// <summary>
        /// Renames raw virtuals that take or return interface pointers to impl_*, adds smart pointer wrappers
        /// with the original names and generates MicroComImpl&lt;T&gt; implementation adapters.
        /// </summary>
        public bool SmartPointerWrappers { get; set; }

        /// <summary>
        /// Smart pointer template used by the wrappers. Must implement the _microcom_* member contract.
        /// </summary>
        public string SmartPointerType { get; set; } = "ComPtr";
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

        public string GenerateCppHeader(CppGenOptions options) => CppGen.GenerateCpp(_idl, options);

        public string GenerateCSharpInterop() => new CSharpGen(_idl).Generate();
    }
}