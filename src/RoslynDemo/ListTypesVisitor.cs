using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDemo
{
    public class ListTypesVisitor
    {
        private static void Write(BaseTypeDeclarationSyntax node)
        {
            var namespaceDeclarationSyntax = node.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
            var typeName = node.Identifier.Text;
            var namespaceName = namespaceDeclarationSyntax?.Name.ToString();
            Console.WriteLine($"{namespaceName}.{typeName}");
        }
    }
}