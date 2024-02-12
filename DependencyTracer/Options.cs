using CommandLine;
using System.IO;

namespace DependencyTracer
{
    /// <summary>
    /// コマンドラインオプション定義
    /// </summary>
    public class Options
    {
        [Option('s', "solution", Required = true, HelpText = "Target solution file (.sln) path")]
        public string SolutionPath { get; set; }

        [Option('c', "class", Required = true, HelpText = "Target class name")]
        public string ClassName { get; set; }

        [Option('m', "method", Required = false, HelpText = "Target method name. If omitted, investigate the any method in target class")]
        public string MethodName { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output file path. If omitted, output to console")]
        public string OutputPath { get; set; }

        [Option('A', "ancestors", Required = false, Default = false, HelpText = "Include indirectly callers")]
        public bool IncludeAncestors { get; set; }

        [Option('L', "leaf", Required = false, Default = false, HelpText = "Output only the highest-level method (not called by any other method)")]
        public bool ShowOnlyLeafSymbols { get; set; }

        [Option('l', "language", Default = LanguageType.CSharp, HelpText = "Development language of target solution")]
        public LanguageType LanguageType { get; set; }

        [Option("verbose", Default = false, HelpText = "Show verbose messages")]
        public bool Verbose { get; set; }

        /// <summary>
        /// 構文チェック以外のオプションの妥当性を検証する
        /// </summary>
        /// <returns></returns>
        public bool IsValid(out string errorMessage)
        {
            if (!File.Exists(SolutionPath))
            {
                errorMessage = "指定されたソリューションファイルが見つかりません";
                return false;
            }

            if (IncludeAncestors && ShowOnlyLeafSymbols)
            {
                errorMessage = "--ancestorsオプションと--leafオプションは併用できません";
                return false;
            }

            // 上記チェックを抜けた場合はOK
            errorMessage = null;
            return true;
        }
    }
}
