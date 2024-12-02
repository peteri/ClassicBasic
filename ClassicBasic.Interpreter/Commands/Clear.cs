// <copyright file="Clear.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the clear command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Clear"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="variableRepository">Variable Repository.</param>
    /// <param name="dataStatementReader">Data statement reader.</param>
    public class Clear(
        IRunEnvironment runEnvironment,
        IVariableRepository variableRepository,
        IDataStatementReader dataStatementReader) : Token("CLEAR", TokenClass.Statement), ICommand
    {
        private readonly IRunEnvironment _runEnvironment = runEnvironment;
        private readonly IVariableRepository _variableRepository = variableRepository;
        private readonly IDataStatementReader _dataStatementReader = dataStatementReader;

        /// <summary>
        /// Executes the CLEAR command.
        /// </summary>
        public void Execute()
        {
            _variableRepository.Clear();
            _runEnvironment.Clear();
            _dataStatementReader.RestoreToLineNumber(null);
        }
    }
}
