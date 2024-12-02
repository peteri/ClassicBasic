// <copyright file="Dim.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DIM command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Dim"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="variableRepository">Variable repository.</param>
    public class Dim(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator,
        IVariableRepository variableRepository) : Token("DIM", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Implements the DIM command.
        /// </summary>
        public void Execute()
        {
            IToken token;
            do
            {
                var name = expressionEvaluator.GetVariableName();
                var indexes = expressionEvaluator.GetIndexes();
                if (indexes.Length > 0)
                {
                    variableRepository.DimensionArray(name, indexes);
                }

                token = runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);
            runEnvironment.CurrentLine.PushToken(token);
        }
    }
}
