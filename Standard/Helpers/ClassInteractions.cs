using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Riverside.Standard.Helpers
{
    public static class ClassInteractions
    {
        public static bool DerivesFrom(TypeSyntax typeSyntax, string baseTypeName, SemanticModel semanticModel)
        {
            var typeSymbol = semanticModel.GetSymbolInfo(typeSyntax).Symbol as INamedTypeSymbol;
            while (typeSymbol != null)
            {
                if (typeSymbol.ToString() == baseTypeName)
                {
                    return true;
                }
                typeSymbol = typeSymbol.BaseType;
            }
            return false;
        }
    }
}
