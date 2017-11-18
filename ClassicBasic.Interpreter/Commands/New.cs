// <copyright file="New.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the new command.
    /// </summary>
    public class New : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IVariableRepository _variableRepository;
        private readonly IDataStatementReader _dataStatementReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="New"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="variableRepository">Variable Repository.</param>
        /// <param name="dataStatementReader">Data statement reader.</param>
        public New(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IVariableRepository variableRepository,
            IDataStatementReader dataStatementReader)
            : base("NEW", TokenClass.Statement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _variableRepository = variableRepository;
            _dataStatementReader = dataStatementReader;
        }

        /// <summary>
        /// Executes the NEW command.
        /// </summary>
        public void Execute()
        {
            _variableRepository.Clear();
            _runEnvironment.Clear();
            _dataStatementReader.RestoreToLineNumber(null);
            _programRepository.Clear();
        }
    }
}
