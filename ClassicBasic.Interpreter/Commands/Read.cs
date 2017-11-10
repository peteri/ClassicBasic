// <copyright file="Read.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the READ command.
    /// </summary>
    public class Read : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IDataStatementReader _dataStatementReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Read"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="dataStatementReader">Data statement reader.</param>
        public Read(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IDataStatementReader dataStatementReader)
            : base("READ", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _dataStatementReader = dataStatementReader;
        }

        /// <summary>
        /// Executes the READ command.
        /// </summary>
        public void Execute()
        {
            var variableReferences = new List<VariableReference>();
            IToken token;
            do
            {
                variableReferences.Add(_expressionEvaluator.GetLeftValue());
                token = _runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);

            _runEnvironment.CurrentLine.PushToken(token);
            _dataStatementReader.ReadInputParser.ReadVariables(variableReferences);
        }
    }
}
