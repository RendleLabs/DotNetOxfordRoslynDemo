using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDemo
{
    static class SyntaxQueryDemo
    {
        public static async Task ListAsync(Solution solution)
        {
            foreach (var document in solution.Projects.SelectMany(p => p.Documents))
            {
                var root = await document.GetSyntaxRootAsync();
                if (root is null) continue;

                foreach (var typeDeclarationSyntax in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    var namespaceDeclarationSyntax = typeDeclarationSyntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();

                    var typeName = typeDeclarationSyntax.Identifier.Text;
                    var namespaceName = namespaceDeclarationSyntax?.Name.ToString();

                    Console.WriteLine($"{namespaceName}.{typeName}");
                }
            }
        }
    }
}