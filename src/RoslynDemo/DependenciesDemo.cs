using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDemo
{
    class DependenciesDemo
    {
        public static async Task ListAllUsedTypes(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                var compilation = await project.GetCompilationAsync();
                if (compilation is null) continue;

                foreach (var document in project.Documents)
                {
                    var tree = await document.GetSyntaxTreeAsync();
                    if (tree is null) continue;
                    
                    var model = compilation.GetSemanticModel(tree, true);
                    if (model is null) continue;

                    var root = await tree.GetRootAsync();

                    foreach (var namedTypeSymbol in GetTypes(root, model).Distinct())
                    {
                        Console.WriteLine($"{namedTypeSymbol.ContainingNamespace}.{namedTypeSymbol.Name}");
                    }
                }
            }
        }
        
        private static IEnumerable<INamedTypeSymbol> GetTypes(SyntaxNode node, SemanticModel model)
        {
            var namedTypes = node.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Select(s => model.GetSymbolInfo(s).Symbol)
                .OfType<INamedTypeSymbol>();

            var expressionTypes = node.DescendantNodes()
                .OfType<ExpressionSyntax>()
                .Select(s => model.GetTypeInfo(s).Type)
                .OfType<INamedTypeSymbol>();

            return namedTypes.Concat(expressionTypes);
        }
    }
}