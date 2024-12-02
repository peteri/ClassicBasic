// <copyright file="Resume.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RESUME command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Resume"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="programRepository">Program repository.</param>
    public class Resume(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository) : Token("RESUME", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the RESUME command.
        /// </summary>
        public void Execute()
        {
            while (runEnvironment.ProgramStack.Count > runEnvironment.LastErrorStackCount)
            {
                runEnvironment.ProgramStack.Pop();
            }

            if (!runEnvironment.LastErrorLine.HasValue)
            {
                runEnvironment.OnErrorGotoLineNumber = null;
                throw new Exceptions.UndefinedStatementException();
            }

            runEnvironment.CurrentLine = programRepository.GetLine(runEnvironment.LastErrorLine.Value);
            runEnvironment.CurrentLine.CurrentToken = runEnvironment.LastErrorToken;
        }
    }
}
