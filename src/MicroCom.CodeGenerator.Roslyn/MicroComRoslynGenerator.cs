using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MicroCom.CodeGenerator.Roslyn
{

    [Generator]
    public class MicroComGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var files = context.AdditionalTextsProvider.Where(t => t.Path.ToLowerInvariant().EndsWith(".mcidl"))
                .Select((t, c) => new
                {
                    path = t.Path,
                    text = t.GetText(c)?.ToString()
                });
            
            context.RegisterSourceOutput(files, (ctx, file) =>
            {
                try
                {
                    var idl = MicroComCodeGenerator.Parse(file.text);
                    var source = idl.GenerateCSharpInterop();
                    ctx.AddSource(Path.GetFileNameWithoutExtension(file.path), source);
                }
                catch (ParseException pe)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("MCOM0001", pe.Message, pe.Message,
                            "MCOM", DiagnosticSeverity.Error,
                            true),
                        Location.Create(file.path, new TextSpan(pe.CharacterOffsetFromStart, 1),
                            new LinePositionSpan(new LinePosition(pe.Line, pe.Position),
                                new LinePosition(pe.Line, pe.Position + 1)))));

                }
                catch (Exception e)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("MCOMERR", "Internal error",
                            e.ToString(),
                            "MCOM", DiagnosticSeverity.Error, true),
                        Location.Create(file.path, new TextSpan(0, 1), new LinePositionSpan(default, default))));
                }
            });
        }
    }
}