// <copyright file="New.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the new command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="New"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="programRepository">Program Repository.</param>
    /// <param name="variableRepository">Variable Repository.</param>
    /// <param name="dataStatementReader">Data statement reader.</param>
    public class New(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository,
        IVariableRepository variableRepository,
        IDataStatementReader dataStatementReader) : Token("NEW", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the NEW command.
        /// </summary>
        public void Execute()
        {
            variableRepository.Clear();
            runEnvironment.Clear();
            dataStatementReader.RestoreToLineNumber(null);
            programRepository.Clear();
        }
    }
}
