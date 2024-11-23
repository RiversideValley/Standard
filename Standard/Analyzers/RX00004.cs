using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Riverside.Standard.Helpers;
using System.Collections.Immutable;

namespace Riverside.Standard.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RX00004 : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostics.RX00004);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
        }

        internal static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.Node;

            if (classDeclaration.Identifier.Text == "App")
            {
                TypeSyntax baseType = classDeclaration.BaseList?.Types.FirstOrDefault()?.Type;
                if (baseType == null || !ClassInteractions.DerivesFrom(baseType, "Microsoft.UI.Xaml.Application", context.SemanticModel))
                {
                    return;
                }

                if (!ClassInteractions.DerivesFrom(baseType, "Riverside.Runtime.Modern.UnifiedApp", context.SemanticModel))
                {
                    Diagnostic diagnostic = Diagnostic.Create(Diagnostics.RX00004, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
