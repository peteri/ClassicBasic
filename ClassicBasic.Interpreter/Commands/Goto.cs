// <copyright file="Goto.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GOTO command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Goto"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="programRepository">Program Repository.</param>
    public class Goto(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository) : Token("GOTO", TokenClass.Statement, TokenType.Goto), ICommand
    {
        /// <summary>
        /// Executes the GOTO command.
        /// </summary>
        public void Execute()
        {
            int? lineNumber = runEnvironment.CurrentLine.GetLineNumber();
            if (lineNumber.HasValue)
            {
                runEnvironment.CurrentLine = programRepository.GetLine(lineNumber.Value);
            }
            else
            {
                throw new Exceptions.UndefinedStatementException();
            }
        }
    }
}