using Microsoft.CodeAnalysis;

namespace Riverside.Standard
{
    public static class Diagnostics
    {
        public static readonly DiagnosticDescriptor RX00001 = new DiagnosticDescriptor(
            "RX00001",
            "Projects must use a stable versioning system",
            "Project '{0}' must use a stable versioning system",
            "Versioning",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that projects use a stable versioning system.");

        public static readonly DiagnosticDescriptor RX00002 = new DiagnosticDescriptor(
            "RX00002",
            "All projects must declare community health files",
            "Project '{0}' must declare community health files",
            "Community",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that all projects declare community health files.");

        public static readonly DiagnosticDescriptor RX00003 = new DiagnosticDescriptor(
            "RX00003",
            "Class libraries should have Riverside Assembly declaration metadata",
            "Class library '{0}' should have Riverside Assembly declaration metadata",
            "Metadata",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that class libraries have Riverside Assembly declaration metadata.");

        public static readonly DiagnosticDescriptor RX00004 = new DiagnosticDescriptor(
            "RX00004",
            "Modern apps must derive their application definition from Riverside.Runtime.Modern.UnifiedApp",
            "Class '{0}' must derive from Riverside.Runtime.Modern.UnifiedApp",
            "Inheritance",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that modern apps derive their application definition from Riverside.Runtime.Modern.UnifiedApp.");

        public static readonly DiagnosticDescriptor RX00005 = new DiagnosticDescriptor(
            "RX00005",
            "All projects must use Riverside Toolkit and Runtime tools and helpers instead of declaring their own",
            "Project '{0}' must use Riverside Toolkit and Runtime tools and helpers",
            "Toolkit",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that all projects use Riverside Toolkit and Runtime tools and helpers instead of declaring their own.");

        public static readonly DiagnosticDescriptor RX00006 = new DiagnosticDescriptor(
            "RX00006",
            "Projects must use Serilog for logging",
            "Project '{0}' must use Serilog for logging",
            "Logging",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that projects use Serilog for logging.");

        public static readonly DiagnosticDescriptor RX00007 = new DiagnosticDescriptor(
            "RX00007",
            "All related projects must be in a monorepo",
            "Project '{0}' must be in a monorepo",
            "Repository",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that all related projects are in a monorepo.");

        public static readonly DiagnosticDescriptor RX00008 = new DiagnosticDescriptor(
            "RX00008",
            "Projects must be localized using localization string resources if they are production ready",
            "Project '{0}' must be localized using localization string resources",
            "Localization",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that projects are localized using localization string resources if they are production ready.");

        public static readonly DiagnosticDescriptor RX00009 = new DiagnosticDescriptor(
            "RX00009",
            "All source files must explicitly define types",
            "Source file '{0}' must explicitly define types",
            "Type Definition",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that all source files explicitly define types.");

        public static readonly DiagnosticDescriptor RX00010 = new DiagnosticDescriptor(
            "RX00010",
            "All source files must be formatted cleanly to Riverside Standard code style",
            "Source file '{0}' must be formatted cleanly to Riverside Standard code style",
            "Code Style",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Ensure that all source files are formatted cleanly to Riverside Standard code style.");
    }
}
