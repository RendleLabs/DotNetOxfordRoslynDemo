using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDemo
{
    static class SymbolDemo
    {
        public static async Task ListAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                var compilation = await project.GetCompilationAsync();
                if (compilation is null) continue;

                foreach (var document in project.Documents)
                {
                    var tree = await document.GetSyntaxTreeAsync();
                    if (tree is null) continue;
                    var model = compilation.GetSemanticModel(tree);

                    var root = await tree.GetRootAsync();
                    if (root is null) continue;

                    foreach (var typeDeclarationSyntax in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                    {
                        var symbol = model.GetDeclaredSymbol(typeDeclarationSyntax);
                        if (symbol is null) continue;
                        Console.WriteLine($"{symbol.ContainingNamespace}.{symbol.Name}");

                        if (symbol is INamedTypeSymbol namedTypeSymbol)
                        {
                            foreach (var methodSymbol in namedTypeSymbol.GetMembers().OfType<IMethodSymbol>())
                            {
                                if (!methodSymbol.IsImplicitlyDeclared)
                                {
                                    Console.WriteLine($"  {methodSymbol.Name}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}