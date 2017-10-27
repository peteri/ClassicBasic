// <copyright file="Let.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the LET command.
    /// </summary>
    public class Let : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Let"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public Let(IRunEnvironment runEnvironment, IExpressionEvaluator expressionEvaluator)
            : base("LET", TokenType.ClassStatement | TokenType.Let)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the LET command.
        /// </summary>
        public void Execute()
        {
            var variableReference = _expressionEvaluator.GetLeftValue();
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.Seperator != TokenType.Equal)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var newValue = _expressionEvaluator.GetExpression();
            variableReference.SetValue(newValue);
        }
    }
}
