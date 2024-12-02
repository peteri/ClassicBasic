// <copyright file="Let.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the LET command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Let"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    public class Let(IRunEnvironment runEnvironment, IExpressionEvaluator expressionEvaluator) : Token("LET", TokenClass.Statement, TokenType.Let), ICommand
    {
        /// <summary>
        /// Executes the LET command.
        /// </summary>
        public void Execute()
        {
            var variableReference = expressionEvaluator.GetLeftValue();
            var token = runEnvironment.CurrentLine.NextToken();
            if (token.Seperator != TokenType.Equal)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var newValue = expressionEvaluator.GetExpression();
            variableReference.SetValue(newValue);
        }
    }
}
