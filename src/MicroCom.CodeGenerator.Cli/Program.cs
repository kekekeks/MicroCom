﻿using System.IO;
using CommandLine;

namespace MicroCom.CodeGenerator.Cli
{
    class Program
    {
        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "Input IDL file")]
            public string Input { get; set; }
            
            [Option("cpp", Required = false, HelpText = "C++ output file")]
            public string CppOutput { get; set; }
            
            [Option("cs", Required = false, HelpText = "C# output file")]
            public string CSharpOutput { get; set; }

        }
        
        static int Main(string[] args)
        {
            var p = Parser.Default.ParseArguments<Options>(args);
            if (p is NotParsed<Options>)
            {
                return 1;
            }

            var opts = ((Parsed<Options>)p).Value;
            
            var text = File.ReadAllText(opts.Input);
            var ast = MicroComCodeGenerator.Parse(text);

            if (opts.CppOutput != null)
                File.WriteAllText(opts.CppOutput, ast.GenerateCppHeader());
            
            if (opts.CSharpOutput != null)
            {
                File.WriteAllText(opts.CSharpOutput, ast.GenerateCSharpInterop());
                
                // HACK: Can't work out how to get the VS project system's fast up-to-date checks
                // to ignore the generated code, so as a workaround set the write time to that of
                // the input.
                File.SetLastWriteTime(opts.CSharpOutput, File.GetLastWriteTime(opts.Input));
            }
            
            return 0;
        }
    }
}
