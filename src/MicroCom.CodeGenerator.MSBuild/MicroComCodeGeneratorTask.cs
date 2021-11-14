using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Microsoft.Build.Framework;

namespace MicroCom.CodeGenerator.MSBuild
{
    public class MicroComCodeGeneratorTask: ITask
    {
        void WriteOutput(string path, string data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, data);
        }
        
        public bool Execute()
        {
            foreach (var i in Inputs)
            {
                Log("Parsing " + i.ItemSpec);
                var idl = MicroComCodeGenerator.Parse(File.ReadAllText(i.ItemSpec));
                var cppOutput = i.GetMetadata("CppHeaderPath");
                if (cppOutput != null)
                {
                    Log("Writing CPP header to " + cppOutput);
                    WriteOutput(cppOutput, idl.GenerateCppHeader());
                    File.SetLastWriteTime(cppOutput, File.GetLastWriteTime(i.ItemSpec));
                }
                
                var interopPath = i.GetMetadata("CSharpInteropPath");
                if (interopPath != null)
                {
                    Log("Writing C# interop code to " + interopPath);
                    new CSharpGen(idl.Idl)
                    {
                        MefAssemblies = ImmutableArray.Create(typeof(CSharpGen).Assembly)
                    }.Generate();
                    WriteOutput(interopPath, idl.GenerateCSharpInterop());
                    File.SetLastWriteTime(interopPath, File.GetLastWriteTime(i.ItemSpec));
                }
            }
            return true;
        }

        void Log(string message)
        {
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message, "", "MicroCom", MessageImportance.Normal));
        }
        
        [Required]
        public ITaskItem[] Inputs { get; set; }
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
    
}