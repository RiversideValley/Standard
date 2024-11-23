using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Riverside.Standard.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RX00006 : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostics.RX00006);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        internal static void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            var compilation = context.Compilation;
            var hasSerilogPackage = compilation.ReferencedAssemblyNames
                .Any(reference => reference.Name.Equals("Serilog", System.StringComparison.OrdinalIgnoreCase));

            if (!hasSerilogPackage)
            {
                var diagnostic = Diagnostic.Create(Diagnostics.RX00006, Location.None, compilation.AssemblyName);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
