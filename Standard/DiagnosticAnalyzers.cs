using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Riverside.Standard.Helpers;
using System.Collections.Immutable;
using Riverside.Standard.Analyzers;

namespace Riverside.Standard
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzers : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            Diagnostics.RX00001,
            Diagnostics.RX00002,
            Diagnostics.RX00003,
            Diagnostics.RX00004,
            Diagnostics.RX00005,
            Diagnostics.RX00006,
            Diagnostics.RX00007,
            Diagnostics.RX00008,
            Diagnostics.RX00009,
            Diagnostics.RX00010
        );

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(RX00001.AnalyzeCompilation);
            context.RegisterCompilationAction(RX00002.AnalyzeCompilation);
            context.RegisterCompilationAction(RX00003.AnalyzeCompilation);
            context.RegisterSyntaxNodeAction(RX00004.AnalyzeNode, SyntaxKind.ClassDeclaration);
            context.RegisterCompilationAction(RX00006.AnalyzeCompilation);
            // Register other analyzers here
        }
    }
}
