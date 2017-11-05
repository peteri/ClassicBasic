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
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dim"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable repository.</param>
        public Dim(
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository)
            : base("DIM", TokenType.ClassStatement)
        {
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
        }

        /// <summary>
        /// Implements the DIM command.
        /// </summary>
        public void Execute()
        {
            var name = _expressionEvaluator.GetVariableName();
            var indexes = _expressionEvaluator.GetIndexes();
            if (indexes.Length > 0)
            {
                _variableRepository.DimensionArray(name, indexes);
            }
        }
    }
}
