// <copyright file="Restore.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RESTORE command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Restore"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run environment.</param>
    /// <param name="dataStatementReader">Data statement reader to use.</param>
    public class Restore(
        IRunEnvironment runEnvironment,
        IDataStatementReader dataStatementReader) : Token("RESTORE", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the RESTORE command.
        /// </summary>
        public void Execute()
        {
            int? lineNumber = runEnvironment.CurrentLine.GetLineNumber();
            dataStatementReader.RestoreToLineNumber(lineNumber);
        }
    }
}
