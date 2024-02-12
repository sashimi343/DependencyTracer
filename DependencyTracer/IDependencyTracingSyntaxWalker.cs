using Microsoft.CodeAnalysis;

namespace DependencyTracer
{
    /// <summary>
    /// 依存関係解析用SyntaxWalkerのインターフェース
    /// </summary>
    public interface IDependencyTracingSyntaxWalker
    {
        void Visit(SyntaxNode rootNode);
    }
}
