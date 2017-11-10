// <copyright file="Dim.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DIM command.
    /// </summary>
    public class Dim : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dim"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable repository.</param>
        public Dim(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository)
            : base("DIM", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
        }

        /// <summary>
        /// Implements the DIM command.
        /// </summary>
        public void Execute()
        {
            IToken token;
            do
            {
                var name = _expressionEvaluator.GetVariableName();
                var indexes = _expressionEvaluator.GetIndexes();
                if (indexes.Length > 0)
                {
                    _variableRepository.DimensionArray(name, indexes);
                }

                token = _runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);
            _runEnvironment.CurrentLine.PushToken(token);
        }
    }
}
