using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace Riverside.Standard.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RX00002 : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostics.RX00002);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        internal static void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            var compilation = context.Compilation;
            var project = context.Options.AdditionalFiles.FirstOrDefault(file => file.Path.EndsWith(".csproj"));

            if (project == null)
            {
                return;
            }

            var projectFile = XDocument.Load(project.Path);
            var outputType = projectFile.Descendants("OutputType").FirstOrDefault()?.Value;

            if (outputType is "WinExe")
            {
                return;
            }

            var readmeItemGroup = projectFile.Descendants("ItemGroup")
                .FirstOrDefault(group => group.Descendants("None")
                .Any(item => item.Attribute("Include")?.Value.IndexOf("README", System.StringComparison.OrdinalIgnoreCase) >= 0));

            if (readmeItemGroup == null)
            {
                var diagnostic = Diagnostic.Create(Diagnostics.RX00002, Location.None, compilation.AssemblyName);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
