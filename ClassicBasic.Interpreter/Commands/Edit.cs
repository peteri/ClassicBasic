// <copyright file="Edit.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the Edit command.
    /// </summary>
    public class Edit : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly ITeletype _teletype;

        /// <summary>
        /// Initializes a new instance of the <see cref="Edit"/> class.
        /// Edit command, allows editing of a program line.
        /// </summary>
        /// <param name="runEnvironment">Run environment.</param>
        /// <param name="programRepository">Program repository</param>
        /// <param name="teletype">teletype.</param>
        public Edit(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            ITeletype teletype)
            : base("EDIT", TokenClass.Statement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _teletype = teletype;
        }

        /// <summary>
        /// Executes the EDIT command.
        /// </summary>
        public void Execute()
        {
            if (_runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDeferredException();
            }

            if (!_teletype.CanEdit)
            {
                throw new Exceptions.UnableToEditException();
            }

            var lineNumber = _runEnvironment.CurrentLine.GetLineNumber();

            if (!lineNumber.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            _teletype.EditText = _programRepository.GetLine(lineNumber.Value).ToString();
        }
    }
}
