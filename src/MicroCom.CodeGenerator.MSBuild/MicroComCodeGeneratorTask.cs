using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;

namespace MicroCom.CodeGenerator.MSBuild
{
    public class MicroComCodeGeneratorTask: ITask
    {
        void WriteOutput(string path, string data)
        {
            path = Path.GetFullPath(path);
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            File.WriteAllText(path, data);
        }

        public bool Execute()
        {
            foreach (var i in Inputs)
            {
                try
                {
                    Log("Parsing " + i.ItemSpec);
                    var idl = MicroComCodeGenerator.Parse(File.ReadAllText(i.ItemSpec));
                    var cppOutput = i.GetMetadata("CppHeaderPath");
                    if (!string.IsNullOrWhiteSpace(cppOutput))
                    {
                        Log("Writing CPP header to " + cppOutput);
                        WriteOutput(cppOutput, idl.GenerateCppHeader());
                        File.SetLastWriteTime(cppOutput, File.GetLastWriteTime(i.ItemSpec));
                    }

                    var interopPath = i.GetMetadata("CSharpInteropPath");
                    if (!string.IsNullOrWhiteSpace(interopPath))
                    {
                        Log("Writing C# interop code to " + interopPath);
                        var output = new CSharpGen(idl.Idl)
                        {
                            RuntimeNamespace = RuntimeNamespace,
                        }.Generate();
                        WriteOutput(interopPath, output);
                        File.SetLastWriteTime(interopPath, File.GetLastWriteTime(i.ItemSpec));
                    }
                }
                catch (ParseException e)
                {
                    BuildEngine.LogErrorEvent(new BuildErrorEventArgs("MCOM", "0001", i.ItemSpec,
                        e.Line, e.Position, e.Line, e.Position,
                        e.Message, "", ""));
                    return false;
                }
            }

            return true;
        }

        void Error(ParseException e)
        {

        }

        void Log(string message)
        {
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message, "", "MicroCom", MessageImportance.High));
        }
        
        [Required]
        public ITaskItem[] Inputs { get; set; }

        [Required]
        public string RuntimeNamespace { get; set; }
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
    
}