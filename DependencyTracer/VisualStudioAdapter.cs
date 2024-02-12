using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyTracer
{
    /// <summary>
    /// Visual Studio(MSBuild)を利用してソリューションファイルへのアクセスを提供するクラス
    /// </summary>
    internal class VisualStudioAdapter : IDisposable
    {
        /// <summary>
        /// ソリューションファイル読み込み時の進捗をコンソールに表示するIProgress実装
        /// </summary>
        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }

        /// <summary>
        /// MSBuildWorkspaceインスタンス
        /// </summary>
        private MSBuildWorkspace _workspace;

        /// <summary>
        /// 最新のMSBuildインスタンスを使用するVisualStudioAdapterを初期化する
        /// </summary>
        public VisualStudioAdapter()
        {
            EnsureRegisterLatestVisualStudioInstance();
            _workspace = MSBuildWorkspace.Create();
            _workspace.WorkspaceFailed += (sender, e) =>
            {
                Console.WriteLine(e.Diagnostic.Message);
            };
        }

        /// <summary>
        /// 指定されたソリューションファイルを開く
        /// </summary>
        /// <param name="solutionPath">ソリューションファイルのパス</param>
        /// <returns>ソリューション</returns>
        public async Task<Solution> OpenSolutionAsync(string solutionPath, bool verbose)
        {
            if (verbose)
            {
                return await _workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
            }
            else
            {
                return await _workspace.OpenSolutionAsync(solutionPath);
            }
        }

        public void Dispose()
        {
            _workspace?.Dispose();
        }

        /// <summary>
        /// システムにインストールされている最新のVisual Studioを使用するようMSBuildLocaterへの登録を行う
        /// 既に登録済みの場合は何もしない
        /// </summary>
        /// <exception cref="InvalidOperationException">Visual Studioインスタンスが見つからない場合</exception>
        private void EnsureRegisterLatestVisualStudioInstance()
        {
            // 既に登録済みなら処理をスキップする
            if (MSBuildLocator.IsRegistered)
            {
                return;
            }

            var instances = MSBuildLocator.QueryVisualStudioInstances().ToList();

            if (instances.Count == 0)
            {
                throw new InvalidOperationException("システムにVisual Studioがインストールされていません。Visual Studioがインストールされている環境で実行してください");
            }
            MSBuildLocator.RegisterInstance(instances.OrderByDescending(i => i.Version).First());
        }

    }
}
