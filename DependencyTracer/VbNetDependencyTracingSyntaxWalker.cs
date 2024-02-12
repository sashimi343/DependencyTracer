using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System;

namespace DependencyTracer
{
    /// <summary>
    /// VB.NETソリューション内の依存関係をたどるためのSyntaxWalker
    /// </summary>
    internal class VbNetDependencyTracingSyntaxWalker : VisualBasicSyntaxWalker, IDependencyTracingSyntaxWalker
    {
        private readonly DependencyList _dependencyList;
        private readonly SemanticModel _semanticModel;
        private readonly bool _verbose;

        public VbNetDependencyTracingSyntaxWalker(DependencyList dependencyList, SemanticModel semanticModel, bool verbose)
        {
            _dependencyList = dependencyList;
            _semanticModel = semanticModel;
            _verbose = verbose;
        }

        /// <summary>
        /// InvocationExpressionを読み込んだ際、呼び出し元・先をDependencyListに追加する
        /// </summary>
        /// <param name="node">SyntaxNode</param>
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var caller = GetCaller(node);
            var callee = GetCallee(node);

            // TODO: メソッド外の呼び出し（プロパティ初期値の代入等）への対応
            if (callee != null)
            {
                _dependencyList.AddDependency(caller, callee);
            }
            else
            {
                if (_verbose)
                {
                    Console.WriteLine("Unsupported callee node: " + node.ToFullString());
                }
            }

            base.VisitInvocationExpression(node);
        }

        private IMethodSymbol GetCallee(InvocationExpressionSyntax node)
        {
            // 呼び出し元がメソッドの場合
            var calleeMethodSymbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            if (calleeMethodSymbol != null)
            {
                return calleeMethodSymbol;
            }

            // TODO: メソッド外の呼び出し（プロパティ初期値の代入等）への対応
            return null;
        }

        private ISymbol GetCaller(SyntaxNode node)
        {
            var currentNode = node;

            while (currentNode != null)
            {
                if (currentNode is MethodBlockSyntax methodDeclarationNode)
                {
                    var methodSymbol = _semanticModel.GetDeclaredSymbol(methodDeclarationNode);
                    return methodSymbol;
                }
                else if (currentNode is ConstructorBlockSyntax constructorDeclarationNode)
                {
                    var constructorSymbol = _semanticModel.GetDeclaredSymbol(constructorDeclarationNode);
                    return constructorSymbol;
                }
                else if (currentNode is VariableDeclaratorSyntax variableDeclaratorNode)
                {
                    var variableSymbol = _semanticModel.GetDeclaredSymbol(variableDeclaratorNode);
                    return variableSymbol;
                }
                else if (currentNode is FieldDeclarationSyntax fieldDeclarationNode)
                {
                    var fieldSymbol = _semanticModel.GetDeclaredSymbol(fieldDeclarationNode);
                    return fieldSymbol;
                }
                else if (currentNode is PropertyStatementSyntax propertyStatementNode)
                {
                    var propertySymbol = _semanticModel.GetDeclaredSymbol(propertyStatementNode);
                    return propertySymbol;
                }
                else if (currentNode is PropertyBlockSyntax propertyBlockNode)
                {
                    var propertySymbol = _semanticModel.GetDeclaredSymbol(propertyBlockNode);
                    return propertySymbol;
                }

                currentNode = currentNode.Parent;
            }

            throw new TraceFailedException(_semanticModel, node);
        }
    }
}
