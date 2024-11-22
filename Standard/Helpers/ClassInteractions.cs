using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Riverside.Standard.Helpers
{
    /// <summary>
    /// Provides helper methods for class interactions.
    /// </summary>
    public static class ClassInteractions
    {
        /// <summary>
        /// Determines whether the specified type syntax derives from the specified base type name.
        /// </summary>
        /// <param name="typeSyntax">The type syntax to check.</param>
        /// <param name="baseTypeName">The fully qualified name of the base type.</param>
        /// <param name="semanticModel">The semantic model used to get symbol information.</param>
        /// <returns><c>true</c> if the specified type syntax derives from the specified base type name; otherwise, <c>false</c>.</returns>
        public static bool DerivesFrom(TypeSyntax typeSyntax, string baseTypeName, SemanticModel semanticModel)
        {
            INamedTypeSymbol typeSymbol = semanticModel.GetSymbolInfo(typeSyntax).Symbol as INamedTypeSymbol;
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
