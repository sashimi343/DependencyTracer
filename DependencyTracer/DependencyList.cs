using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace DependencyTracer
{
    /// <summary>
    /// 依存関係の一覧を保持するクラス
    /// </summary>
    public class DependencyList
    {
        private readonly List<DependencyInfo> _dependencies;

        /// <summary>
        /// このクラスのインスタンスを初期化する
        /// </summary>
        public DependencyList()
        {
            _dependencies = new List<DependencyInfo>();
        }

        /// <summary>
        /// 依存関係を追加する
        /// </summary>
        /// <param name="caller">呼び出し元シンボル</param>
        /// <param name="callee">呼び出し先メソッドシンボル</param>
        public void AddDependency(ISymbol caller, IMethodSymbol callee)
        {
            _dependencies.Add(new DependencyInfo(caller, callee));
        }

        /// <summary>
        /// 指定したクラスを直接呼び出すシンボルの一覧を取得する
        /// </summary>
        /// <param name="className">クラス名（名前空間ありの名称）</param>
        /// <returns>指定したクラスを直接利用するメソッド・クラスのシンボル一覧</returns>
        public IEnumerable<ISymbol> FindCallersByClassName(string className)
        {
            return _dependencies
                .Where(d => d.IsMatchCalleeClass(className))
                .Select(d => d.Caller);
        }

        /// <summary>
        /// 指定したクラスを呼び出すシンボルの一覧を再帰的に取得する
        /// </summary>
        /// <param name="className">クラス名（名前空間ありの名称）</param>
        /// <returns>指定したクラスを直接または間接的に利用するメソッド・クラスのシンボル一覧</returns>
        public IEnumerable<ISymbol> FindCallersByClassNameRecursively(string className)
        {
            var parentCallers = FindCallersByClassName(className);
            var recursiveCallers = parentCallers.SelectMany(c => FindCallersByMethodNameRecursively(c.GetFullClassName(), c.Name));
            return parentCallers.Concat(recursiveCallers);
        }

        /// <summary>
        /// 指定したメソッドを直接呼び出すシンボルの一覧を取得する
        /// </summary>
        /// <param name="className">クラス名（名前空間ありの名称）</param>
        /// <param name="methodName">メソッド名</param>
        /// <returns>指定したメソッドを直接利用するメソッド・クラスのシンボル一覧</returns>
        public IEnumerable<ISymbol> FindCallersByMethodName(string className, string methodName)
        {
            return _dependencies
                .Where(d => d.IsMatchCalleeMethod(className, methodName))
                .Select(d => d.Caller);
        }

        /// <summary>
        /// 指定したメソッドを呼び出すシンボルの一覧を再帰的に取得する
        /// </summary>
        /// <param name="className">クラス名（名前空間ありの名称）</param>
        /// <param name="methodName">メソッド名</param>
        /// <returns>指定したメソッドを直接または間接的に利用するメソッド・クラスのシンボル一覧</returns>
        public IEnumerable<ISymbol> FindCallersByMethodNameRecursively(string className, string methodName)
        {
            var parentCallers = FindCallersByMethodName(className, methodName);
            var recursiveCallers = parentCallers.SelectMany(c => FindCallersByMethodNameRecursively(c.GetFullClassName(), c.Name));
            return parentCallers.Concat(recursiveCallers);
        }

        /// <summary>
        /// 引数のシンボル一覧について、呼び出し先になっていない（依存関係の最上位に位置する）シンボルのみに絞り込みを行う
        /// </summary>
        /// <param name="symbols">絞り込み対象のシンボル一覧</param>
        /// <returns>依存関係最上位に位置するシンボルの一覧</returns>
        public IEnumerable<ISymbol> ExtractLeafSymbols(IEnumerable<ISymbol> symbols)
        {
            return symbols.Where(s => !_dependencies.Any(d => d.IsMatchCalleeMethod(s.GetFullClassName(), s.Name)));
        }
    }
}
