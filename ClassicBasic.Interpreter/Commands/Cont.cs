// <copyright file="Cont.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the CONT command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Cont"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="programRepository">Program Repository.</param>
    public class Cont(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository) : Token("CONT", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the CONT command. Note this will crash the program into
        /// an endless loop if used in a program.
        /// </summary>
        public void Execute()
        {
            if (runEnvironment.ContinueLineNumber == null)
            {
                throw new Exceptions.CantContinueException();
            }

            runEnvironment.CurrentLine = programRepository.GetLine(runEnvironment.ContinueLineNumber.Value);
            runEnvironment.CurrentLine.CurrentToken = runEnvironment.ContinueToken;
        }
    }
}
