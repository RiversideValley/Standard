using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Riverside.Standard.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RX00001 : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostics.RX00001);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        internal static void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            var compilation = context.Compilation;
            var hasSemanticsPackage = compilation.ReferencedAssemblyNames
                .Any(reference => reference.Name.Equals("Semantics", System.StringComparison.OrdinalIgnoreCase)); // TODO: Replace Semantics with Riverside.Standard.Semantics or Riverside.Runtime.Semantics

            if (!hasSemanticsPackage)
            {
                var diagnostic = Diagnostic.Create(Diagnostics.RX00001, Location.None, compilation.AssemblyName);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
