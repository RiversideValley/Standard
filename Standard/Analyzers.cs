using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Riverside.Standard.Helpers;
using System.Collections.Immutable;

namespace Riverside.Standard
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzers : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RVSX0001";
        private static readonly LocalizableString Title = "Class must derive from Riverside.Runtime.AppEx";
        private static readonly LocalizableString MessageFormat = "Class '{0}' must derive from Riverside.Runtime.AppEx";
        private static readonly LocalizableString Description = "Ensure that App class derives from Riverside.Runtime.AppEx.";
        private const string Category = "Naming";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            // Check if the class is named "App"
            if (classDeclaration.Identifier.Text == "App")
            {
                // Check if the class derives from "Microsoft.UI.Xaml.Application"
                var baseType = classDeclaration.BaseList?.Types.FirstOrDefault()?.Type;
                if (baseType == null || !ClassInteractions.DerivesFrom(baseType, "Microsoft.UI.Xaml.Application", context.SemanticModel))
                {
                    return;
                }

                // Check if the class derives from "Riverside.Runtime.AppEx"
                if (!ClassInteractions.DerivesFrom(baseType, "Riverside.Runtime.AppEx", context.SemanticModel))
                {
                    var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
