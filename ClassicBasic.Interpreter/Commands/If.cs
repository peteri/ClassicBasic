// <copyright file="If.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the IF command.
    /// </summary>
    public class If : Token, ICommand, IRepeatExecuteCommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="If"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program repository.</param>
        public If(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository)
            : base("IF", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the IF statement.
        /// </summary>
        public void Execute()
        {
            var result = _expressionEvaluator.GetExpression();
            var token = _runEnvironment.CurrentLine.NextToken();
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
                while (!_runEnvironment.CurrentLine.EndOfLine
                        && (token.Statement != TokenType.Else))
                {
                    token = _runEnvironment.CurrentLine.NextToken();
                }
            }
            else
            {
                // We have a winner, just a line number?
                int? lineNumber = _expressionEvaluator.GetLineNumber();
                if (lineNumber.HasValue)
                {
                    // Go there
                    _runEnvironment.CurrentLine = _programRepository.GetLine(lineNumber.Value);
                }
            }
        }
    }
}
