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
            // Outputs get the latest input timestamp, so the incremental build sees them as up to date
            DateTime GetTimestamp(ITaskItem idlItem)
            {
                var rv = File.GetLastWriteTime(idlItem.ItemSpec);
                foreach (var additional in AdditionalInputs ?? Array.Empty<ITaskItem>())
                {
                    if (File.Exists(additional.ItemSpec))
                    {
                        var time = File.GetLastWriteTime(additional.ItemSpec);
                        if (time > rv)
                            rv = time;
                    }
                }

                return rv;
            }

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
                        bool.TryParse(i.GetMetadata("CppSmartPointerWrappers"), out var smartPointerWrappers);
                        WriteOutput(cppOutput, idl.GenerateCppHeader(new CppGenOptions
                        {
                            SmartPointerWrappers = smartPointerWrappers,
                            SmartPointerType = i.GetMetadata("CppSmartPointerType")
                        }));
                        File.SetLastWriteTime(cppOutput, GetTimestamp(i));
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
                        File.SetLastWriteTime(interopPath, GetTimestamp(i));
                    }
                }
                catch (ParseException e)
                {
                    BuildEngine.LogErrorEvent(new BuildErrorEventArgs("MCOM", "0001", i.ItemSpec,
                        e.Line, e.Position, e.Line, e.Position,
                        e.Message, "", ""));
                    return false;
                }
                catch (CodeGenException e)
                {
                    BuildEngine.LogErrorEvent(new BuildErrorEventArgs("MCOM", "0002", i.ItemSpec,
                        0, 0, 0, 0, e.Message, "", ""));
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

        public ITaskItem[] AdditionalInputs { get; set; }
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
    
}