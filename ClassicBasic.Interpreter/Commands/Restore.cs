// <copyright file="Restore.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RESTORE command.
    /// </summary>
    public class Restore : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IDataStatementReader _dataStatementReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Restore"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run environment.</param>
        /// <param name="dataStatementReader">Data statement reader to use.</param>
        public Restore(IRunEnvironment runEnvironment, IDataStatementReader dataStatementReader)
            : base("RESTORE", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _dataStatementReader = dataStatementReader;
        }

        /// <summary>
        /// Executes the RESTORE command.
        /// </summary>
        public void Execute()
        {
            int? lineNumber = _runEnvironment.CurrentLine.GetLineNumber();
            _dataStatementReader.RestoreToLineNumber(lineNumber);
        }
    }
}
