// <copyright file="If.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the IF command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="If"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="programRepository">Program repository.</param>
    public class If(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator,
        IProgramRepository programRepository) : Token("IF", TokenClass.Statement), ICommand, IRepeatExecuteCommand
    {
        /// <summary>
        /// Executes the IF statement.
        /// </summary>
        public void Execute()
        {
            var result = expressionEvaluator.GetExpression();
            var token = runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.Then && token.Statement != TokenType.Goto)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            bool test = (result.Type == typeof(string))
                ? (result.ValueAsString() == string.Empty)
                : (result.ValueAsDouble() == 0.0);

            // Check the result
            if (test)
            {
                // Skip to the ELSE or EndOfLine
                while (!runEnvironment.CurrentLine.EndOfLine
                        && (token.Statement != TokenType.Else))
                {
                    token = runEnvironment.CurrentLine.NextToken();
                }
            }
            else
            {
                // We have a winner, just a line number?
                int? lineNumber = runEnvironment.CurrentLine.GetLineNumber();
                if (lineNumber.HasValue)
                {
                    // Go there
                    runEnvironment.CurrentLine = programRepository.GetLine(lineNumber.Value);
                }
            }
        }
    }
}
