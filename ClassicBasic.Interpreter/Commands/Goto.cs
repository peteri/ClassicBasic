// <copyright file="Goto.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GOTO command.
    /// </summary>
    public class Goto : Token, ICommand
    {
        private readonly IProgramRepository _programRepository;
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Goto"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program Repository.</param>
        public Goto(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository)
            : base("GOTO", TokenType.ClassStatement | TokenType.Goto)
        {
            _programRepository = programRepository;
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the GOTO command.
        /// </summary>
        public void Execute()
        {
            int? lineNumber = _runEnvironment.CurrentLine.GetLineNumber();
            if (lineNumber.HasValue)
            {
                _runEnvironment.CurrentLine = _programRepository.GetLine(lineNumber.Value);
            }
            else
            {
                throw new Exceptions.UndefinedStatementException();
            }
        }
    }
}