// <copyright file="Cont.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the CONT command.
    /// </summary>
    public class Cont : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cont"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        public Cont(IRunEnvironment runEnvironment, IProgramRepository programRepository)
            : base("CONT", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the CONT command. Note this will crash the program into
        /// an endless loop if used in a program.
        /// </summary>
        public void Execute()
        {
            if (_runEnvironment.ContinueLineNumber == null)
            {
                throw new Exceptions.CantContinueException();
            }

            _runEnvironment.CurrentLine = _programRepository.GetLine(_runEnvironment.ContinueLineNumber.Value);
            _runEnvironment.CurrentLine.CurrentToken = _runEnvironment.ContinueToken;
        }
    }
}
