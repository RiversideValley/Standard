using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Riverside.Standard;

namespace Riverside.Standard.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RX00010 : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostics.RX00010);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            // Register analysis actions here
        }
    }
}
