// <copyright file="Edit.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the Edit command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Edit"/> class.
    /// Edit command, allows editing of a program line.
    /// </remarks>
    /// <param name="runEnvironment">Run environment.</param>
    /// <param name="programRepository">Program repository.</param>
    /// <param name="teletype">teletype.</param>
    public class Edit(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository,
        ITeletype teletype) : Token("EDIT", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the EDIT command.
        /// </summary>
        public void Execute()
        {
            if (runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDeferredException();
            }

            if (!teletype.CanEdit)
            {
                throw new Exceptions.UnableToEditException();
            }

            var lineNumber = runEnvironment.CurrentLine.GetLineNumber();

            if (!lineNumber.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            teletype.EditText = programRepository.GetLine(lineNumber.Value).ToString();
        }
    }
}
