using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace Riverside.Standard.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RX00003 : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostics.RX00003);

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

            var requiredProperties = new[]
            {
                "GeneratePackageOnBuild",
                "Authors",
                "Company",
                "Copyright",
                "PackageReadmeFile",
                "RepositoryUrl",
                "PackageTags",
                "Description",
                "PackageLicenseExpression",
                "PackageRequireLicenseAcceptance",
                "IncludeSymbols",
                "SymbolPackageFormat"
            };

            var propertyGroup = projectFile.Descendants("PropertyGroup").FirstOrDefault();
            if (propertyGroup == null)
            {
                ReportDiagnostic(context, compilation.AssemblyName);
                return;
            }

            foreach (var property in requiredProperties)
            {
                var element = propertyGroup.Element(property);
                if (element == null || string.IsNullOrWhiteSpace(element.Value))
                {
                    ReportDiagnostic(context, compilation.AssemblyName);
                    return;
                }

                if (property == "PackageTags" && !element.Value.Contains("riverside"))
                {
                    ReportDiagnostic(context, compilation.AssemblyName);
                    return;
                }
            }
        }

        private static void ReportDiagnostic(CompilationAnalysisContext context, string assemblyName)
        {
            var diagnostic = Diagnostic.Create(Diagnostics.RX00003, Location.None, assemblyName);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
