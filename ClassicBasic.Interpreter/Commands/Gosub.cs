// <copyright file="Gosub.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GOSUB command.
    /// </summary>
    public class Gosub : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gosub"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program Repository.</param>
        public Gosub(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository)
            : base("GOSUB", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the GOSUB command.
        /// </summary>
        public void Execute()
        {
            var nextLine = _expressionEvaluator.GetLineNumber();
            if (!nextLine.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var returnAddress = new StackEntry
            {
                Line = _runEnvironment.CurrentLine,
                LineToken = _runEnvironment.CurrentLine.CurrentToken
            };

            _runEnvironment.CurrentLine = _programRepository.GetLine(nextLine.Value);
            _runEnvironment.ProgramStack.Push(returnAddress);
        }
    }
}
