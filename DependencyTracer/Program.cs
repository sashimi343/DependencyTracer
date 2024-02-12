using CommandLine;
using CommandLine.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DependencyTracer
{
    internal class Program
    {
        private Options _options;

        static void Main(string[] args)
        {
            var parser = new Parser(config =>
            {
                config.AutoVersion = true;
                config.AutoHelp = true;
                config.CaseInsensitiveEnumValues = true;
                config.HelpWriter = null;
            });
            var result = parser.ParseArguments<Options>(args);

            try
            {
                result.WithParsed(options => new Program().Run(options))
                    .WithNotParsed(errors => new Program().DisplayErrors(result, errors));
            }
            catch (TraceFailedException e)
            {
                Console.WriteLine("依存関係解析に失敗しました。恐らくバグなので以下の情報を開発者に送付してください。");
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine(e.SyntaxNode.ToFullString());
                Console.WriteLine("");
                Console.WriteLine(e.SyntaxNode?.SyntaxTree.ToString());
                Console.WriteLine("------------------------------------------------------------------------");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal void Run(Options options)
        {
            _options = options;

            if (!_options.IsValid(out string errorMessage))
            {
                Console.WriteLine(errorMessage);
                return;
            }

            var symbols = GetDependencies();

            OutputResult(symbols);
        }

        internal void DisplayErrors(ParserResult<Options> result, IEnumerable<Error> errors)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.AddEnumValuesToHelpText = true;

                return h;
            }, e => e);

            Console.WriteLine(helpText);
        }

        internal IEnumerable<ISymbol> GetDependencies()
        {
            using (var visualStudio = new VisualStudioAdapter())
            {
                var solution = visualStudio.OpenSolutionAsync(_options.SolutionPath, _options.Verbose).Result;
                var documents = solution.Projects.SelectMany(p => p.Documents);
                var dependencyList = new DependencyList();
                var factory = new DependencyTracingSyntaxWalkerFactory(_options.LanguageType, _options.Verbose);

                foreach (var document in documents)
                {
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    var syntaxTree = document.GetSyntaxTreeAsync().Result;
                    var walker = factory.CreateSyntaxWalker(dependencyList, semanticModel);
                    walker.Visit(syntaxTree.GetRoot());
                }

                var symbols = FilterDependencies(dependencyList);
                return symbols;
            }
        }

        private IEnumerable<ISymbol> FilterDependencies(DependencyList dependencyList)
        {
            IEnumerable<ISymbol> symbols;
            bool recursive = _options.IncludeAncestors || _options.ShowOnlyLeafSymbols;

            if (string.IsNullOrEmpty(_options.MethodName))
            {
                if (recursive)
                {
                    symbols = dependencyList.FindCallersByClassNameRecursively(_options.ClassName);
                }
                else
                {
                    symbols = dependencyList.FindCallersByClassName(_options.ClassName);
                }
            }
            else
            {
                if (recursive)
                {
                    symbols = dependencyList.FindCallersByMethodNameRecursively(_options.ClassName, _options.MethodName);
                }
                else
                {
                    symbols = dependencyList.FindCallersByMethodName(_options.ClassName, _options.MethodName);
                }
            }

            if (_options.ShowOnlyLeafSymbols)
            {
                symbols = dependencyList.ExtractLeafSymbols(symbols);
            }

            return symbols.Distinct(SymbolEqualityComparer.Default);
        }

        private void OutputResult(IEnumerable<ISymbol> symbols)
        {
            var output = new StringBuilder();

            // ヘッダ行の出力
            output.AppendLine("\"ClassName\",\"MethodName\"");

            // ボディの出力
            foreach (var symbol in symbols)
            {
                output.Append("\"");
                output.Append(symbol.GetFullClassName());
                output.Append("\"");
                output.Append(",");
                output.Append("\"");
                output.Append(symbol.Name);
                output.AppendLine("\"");
            }

            if (string.IsNullOrEmpty(_options.OutputPath))
            {
                Console.WriteLine(output.ToString());
            }
            else
            {
                File.WriteAllText(_options.OutputPath, output.ToString());
            }
        }
    }
}
