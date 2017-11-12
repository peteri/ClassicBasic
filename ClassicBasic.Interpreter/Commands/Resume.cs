// <copyright file="Resume.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RESUME command.
    /// </summary>
    public class Resume : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resume"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository"></param>
        public Resume(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository)
            : base("RESUME", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the RESUME command.
        /// </summary>
        public void Execute()
        {
            while (_runEnvironment.ProgramStack.Count > _runEnvironment.LastErrorStackCount)
            {
                _runEnvironment.ProgramStack.Pop();
            }

            if (_runEnvironment.LastErrorLine.HasValue)
            {
                _runEnvironment.CurrentLine = _programRepository.GetLine(_runEnvironment.LastErrorLine.Value);
                _runEnvironment.CurrentLine.CurrentToken = _runEnvironment.LastErrorToken;
            }
            else
            {
                _runEnvironment.OnErrorGotoLineNumber = null;
                throw new Exceptions.UndefinedStatementException();
            }
        }
    }
}
