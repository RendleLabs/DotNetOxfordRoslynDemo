using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RoslynDemo
{
    static class RewriterDemo
    {
        public static async Task UpshiftAllStrings(Workspace workspace)
        {
            var newSolution = workspace.CurrentSolution;

            int count = 0;

            foreach (var project in workspace.CurrentSolution.Projects)
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
                    
                    var upshifter = new StringLiteralUpshifter(model);

                    var newRoot = upshifter.Visit(root);

                    if (!newRoot.IsEquivalentTo(root))
                    {
                        ++count;
                        newSolution = newSolution.WithDocumentSyntaxRoot(document.Id, newRoot);
                    }
                }
            }

            if (!ReferenceEquals(newSolution, workspace.CurrentSolution))
            {
                workspace.TryApplyChanges(newSolution);
                Console.Write($"{count} changes applied.");
            }
        }
    }
}