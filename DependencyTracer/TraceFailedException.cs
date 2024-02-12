using Microsoft.CodeAnalysis;
using System;

namespace DependencyTracer
{
    /// <summary>
    /// 依存関係解析中に発生した例外
    /// </summary>
    public class TraceFailedException : Exception
    {
        public static readonly string ExceptionMessage = "Failed to trace dependencies.";

        public SemanticModel SemanticModel { get; private set; }

        public SyntaxNode SyntaxNode { get; private set; }

        public TraceFailedException(SemanticModel semanticModel, SyntaxNode syntaxNode)
            : base(ExceptionMessage)
        {
            SemanticModel = semanticModel;
            SyntaxNode = syntaxNode;
        }
    }
}
