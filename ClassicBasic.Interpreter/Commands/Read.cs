// <copyright file="Read.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the READ command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Read"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="dataStatementReader">Data statement reader.</param>
    public class Read(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator,
        IDataStatementReader dataStatementReader) : Token("READ", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the READ command.
        /// </summary>
        public void Execute()
        {
            var variableReferences = new List<VariableReference>();
            IToken token;
            do
            {
                variableReferences.Add(expressionEvaluator.GetLeftValue());
                token = runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);

            runEnvironment.CurrentLine.PushToken(token);
            runEnvironment.DataErrorLine = dataStatementReader.CurrentDataLine;
            dataStatementReader.ReadInputParser.ReadVariables(variableReferences);
            runEnvironment.DataErrorLine = null;
        }
    }
}
