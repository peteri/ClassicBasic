// <copyright file="Clear.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the clear command.
    /// </summary>
    public class Clear : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IVariableRepository _variableRepository;
        private readonly IDataStatementReader _dataStatementReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Clear"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="variableRepository">Variable Repository.</param>
        /// <param name="dataStatementReader">Data statement reader.</param>
        public Clear(
            IRunEnvironment runEnvironment,
            IVariableRepository variableRepository,
            IDataStatementReader dataStatementReader)
            : base("CLEAR", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _variableRepository = variableRepository;
            _dataStatementReader = dataStatementReader;
        }

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
