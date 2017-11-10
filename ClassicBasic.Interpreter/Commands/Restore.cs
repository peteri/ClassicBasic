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
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IDataStatementReader _dataStatementReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Restore"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="dataStatementReader">Data statement reader to use.</param>
        public Restore(IExpressionEvaluator expressionEvaluator, IDataStatementReader dataStatementReader)
            : base("RESTORE", TokenType.ClassStatement)
        {
            _expressionEvaluator = expressionEvaluator;
            _dataStatementReader = dataStatementReader;
        }

        /// <summary>
        /// Executes the RESTORE command.
        /// </summary>
        public void Execute()
        {
            int? lineNumber = _expressionEvaluator.GetLineNumber();
            _dataStatementReader.RestoreToLineNumber(lineNumber);
        }
    }
}
