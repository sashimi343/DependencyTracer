using Microsoft.CodeAnalysis;

namespace DependencyTracer
{
    /// <summary>
    /// ISymbolの拡張メソッド
    /// </summary>
    public static class ISymbolExtension
    {
        /// <summary>
        /// 名前空間付きの正式なクラス名を取得する
        /// </summary>
        /// <param name="symbol">ISybmol</param>
        /// <returns>正式なクラス名</returns>
        public static string GetFullClassName(this ISymbol symbol)
        {
            if (symbol.ContainingNamespace != null && !string.IsNullOrEmpty(symbol.ContainingNamespace.Name))
            {
                return symbol.ContainingNamespace.Name + "." + symbol.ContainingType.Name;
            }
            else
            {
                return symbol.ContainingType.Name;
            }
        }
    }
}
