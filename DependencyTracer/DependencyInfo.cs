using Microsoft.CodeAnalysis;

namespace DependencyTracer
{
    /// <summary>
    /// 依存関係を表す情報を保持するクラス
    /// </summary>
    public class DependencyInfo
    {
        /// <summary>
        /// 呼び出し元シンボル
        /// </summary>
        public ISymbol Caller { get; }

        /// <summary>
        /// 呼び出し先メソッドシンボル
        /// </summary>
        public IMethodSymbol Callee { get; }

        /// <summary>
        /// このクラスのインスタンスを生成する
        /// </summary>
        /// <param name="caller">呼び出し元シンボル</param>
        /// <param name="callee">呼び出し先メソッドシンボル</param>
        public DependencyInfo(ISymbol caller, IMethodSymbol callee)
        {
            Caller = caller;
            Callee = callee;
        }

        /// <summary>
        /// 呼び出し先クラス名が引数と一致するかチェックする
        /// </summary>
        /// <param name="className">クラス名（名前空間ありの名称）</param>
        /// <returns>チェック結果</returns>
        public bool IsMatchCalleeClass(string className)
        {
            return Callee.GetFullClassName() == className;
        }

        /// <summary>
        /// 呼び出し先クラス名・メソッド名が引数と一致するかチェックする
        /// </summary>
        /// <param name="className">クラス名（名前空間ありの名称）</param>
        /// <param name="methodName">メソッド名</param>
        /// <returns>チェック結果</returns>
        public bool IsMatchCalleeMethod(string className, string methodName)
        {
            return Callee.GetFullClassName() == className && Callee.Name == methodName;
        }
    }
}
