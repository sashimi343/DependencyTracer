using Microsoft.CodeAnalysis;
using System;

namespace DependencyTracer
{
    /// <summary>
    /// DependencyTracingSyntaxWalkerのインスタンスを生成するためのファクトリ
    /// </summary>
    public class DependencyTracingSyntaxWalkerFactory
    {
        private readonly LanguageType _languageType;
        private readonly bool _verbose;

        public DependencyTracingSyntaxWalkerFactory(LanguageType languageType, bool verbose)
        {
            _languageType = languageType;
            _verbose = verbose;
        }

        /// <summary>
        /// 新しいIDependencyTracingSyntaxWalkerを生成する
        /// </summary>
        /// <param name="dependencyList">依存関係リスト</param>
        /// <param name="semanticModel">解析対象のセマンティックモデル</param>
        /// <returns>IDependencyTracingSyntaxWalker</returns>
        public IDependencyTracingSyntaxWalker CreateSyntaxWalker(DependencyList dependencyList, SemanticModel semanticModel)
        {
            switch (_languageType)
            {
                case LanguageType.CSharp:
                    return new CSharpDependencyTracingSyntaxWalker(dependencyList, semanticModel, _verbose);
                case LanguageType.VbNet:
                    return new VbNetDependencyTracingSyntaxWalker(dependencyList, semanticModel, _verbose);
                default:
                    throw new NotSupportedException("Unsupported language type: " + _languageType);
            }
        }
    }
}
