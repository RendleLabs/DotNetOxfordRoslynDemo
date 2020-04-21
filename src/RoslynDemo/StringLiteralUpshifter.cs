using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynDemo
{
    public class StringLiteralUpshifter
    {
        private readonly SemanticModel _model;

        public StringLiteralUpshifter(SemanticModel model)
        {
            _model = model;
        }
    }
}