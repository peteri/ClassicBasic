// <copyright file="Def.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DEF command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Def"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    public class Def(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator) : Token("DEF", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the DEF command.
        /// </summary>
        public void Execute()
        {
            if (!runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            var token = runEnvironment.CurrentLine.NextToken();
            var nameToken = runEnvironment.CurrentLine.NextToken();
            var bracketToken = runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.Fn
                || nameToken.TokenClass != TokenClass.Variable
                || bracketToken.Seperator != TokenType.OpenBracket)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var variableName = expressionEvaluator.GetVariableName();
            bracketToken = runEnvironment.CurrentLine.NextToken();
            var equalsToken = runEnvironment.CurrentLine.NextToken();
            if (bracketToken.Seperator != TokenType.CloseBracket || equalsToken.Seperator != TokenType.Equal)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var userFunctionDefinition = new UserDefinedFunction
            {
                Line = runEnvironment.CurrentLine,
                LineToken = runEnvironment.CurrentLine.CurrentToken,
                FunctionName = nameToken.Text,
                VariableName = variableName,
            };

            runEnvironment.DefinedFunctions[userFunctionDefinition.FunctionName] = userFunctionDefinition;

            while (!runEnvironment.CurrentLine.EndOfLine)
            {
                token = runEnvironment.CurrentLine.NextToken();
                if (token.Seperator == TokenType.Colon)
                {
                    runEnvironment.CurrentLine.PushToken(token);
                    return;
                }
            }
        }
    }
}
