// <copyright file="Def.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DEF command.
    /// </summary>
    public class Def : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Def"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public Def(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExpressionEvaluator expressionEvaluator)
            : base("DEF", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the DEF command.
        /// </summary>
        public void Execute()
        {
            if (!_runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            var token = _runEnvironment.CurrentLine.NextToken();
            var nameToken = _runEnvironment.CurrentLine.NextToken();
            var bracketToken = _runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.Fn
                || nameToken.TokenClass != TokenType.ClassVariable
                || bracketToken.Seperator != TokenType.OpenBracket)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var variableName = _expressionEvaluator.GetVariableName();
            bracketToken = _runEnvironment.CurrentLine.NextToken();
            var equalsToken = _runEnvironment.CurrentLine.NextToken();
            if (bracketToken.Seperator != TokenType.CloseBracket || equalsToken.Seperator != TokenType.Equal)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var userFunctionDefinition = new UserDefinedFunction
            {
                Line = _runEnvironment.CurrentLine,
                LineToken = _runEnvironment.CurrentLine.CurrentToken,
                FunctionName = nameToken.Text,
                VariableName = variableName
            };

            _runEnvironment.DefinedFunctions[userFunctionDefinition.FunctionName] = userFunctionDefinition;

            while (!_runEnvironment.CurrentLine.EndOfLine)
            {
                token = _runEnvironment.CurrentLine.NextToken();
                if (token.Statement == TokenType.Colon)
                {
                    _runEnvironment.CurrentLine.PushToken(token);
                    return;
                }
            }
        }
    }
}
