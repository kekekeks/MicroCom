using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MicroCom.CodeGenerator.Roslyn
{
    [Generator]
    public class MicroComGenerator : IIncrementalGenerator
    {
        private const string IsMicroComIdlMetadata = "build_metadata.AdditionalFiles.IsMicroComIdl";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var files = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(t =>
                {
                    var (file, optionsProvider) = t;

                    if (optionsProvider.GetOptions(file).TryGetValue(IsMicroComIdlMetadata, out var isMicroComIdl)
                        && bool.TryParse(isMicroComIdl, out var isIdl) && isIdl)
                    {
                        return true;
                    }

                    return false;
                })
                .Select((t, c) => new
                {
                    path = t.Left.Path,
                    text = t.Left.GetText(c)?.ToString()
                });

            context.RegisterSourceOutput(files, (ctx, file) =>
            {
                try
                {
                    var idl = MicroComCodeGenerator.Parse(file.text);
                    var source = idl.GenerateCSharpInterop();
                    ctx.AddSource(Path.GetFileNameWithoutExtension(file.path) + ".g.cs", source);
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