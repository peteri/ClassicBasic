// <copyright file="OnErr.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ONERR command.
    /// </summary>
    public class OnErr : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnErr"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public OnErr(IRunEnvironment runEnvironment, IExpressionEvaluator expressionEvaluator)
            : base("ONERR", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the ONERR command.
        /// </summary>
        public void Execute()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            var lineNumber = _expressionEvaluator.GetLineNumber();
            if (token.Statement != TokenType.Goto || !lineNumber.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            _runEnvironment.OnErrorGotoLineNumber = (lineNumber.Value == 0) ? (int?)null : lineNumber.Value;
        }
    }
}
