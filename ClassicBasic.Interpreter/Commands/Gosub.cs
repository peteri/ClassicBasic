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
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gosub"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        public Gosub(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository)
            : base("GOSUB", TokenClass.Statement, TokenType.Gosub)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the GOSUB command.
        /// </summary>
        public void Execute()
        {
            var nextLine = _runEnvironment.CurrentLine.GetLineNumber();
            if (!nextLine.HasValue)
            {
                throw new Exceptions.UndefinedStatementException();
            }

            var returnAddress = new StackEntry
            {
                Line = _runEnvironment.CurrentLine,
                LineToken = _runEnvironment.CurrentLine.CurrentToken
            };

            _runEnvironment.CurrentLine = _programRepository.GetLine(nextLine.Value);
            _runEnvironment.ProgramStack.Push(returnAddress);
            _runEnvironment.TestForStackOverflow();
        }
    }
}
