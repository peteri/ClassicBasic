// <copyright file="IExpressionEvaluator.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Expression evaluator.
    /// </summary>
    public interface IExpressionEvaluator
    {
        /// <summary>
        /// Gets an expression.
        /// </summary>
        /// <returns>Accumlator with either a string or double.</returns>
        Accumulator GetExpression();

        /// <summary>
        /// Gets a variable reference which can be used a LValue for assignments.
        /// </summary>
        /// <returns>Reference to a variable.</returns>
        VariableReference GetLeftValue();

        /// <summary>
        /// Parses an array of indexes from the current command stream.
        /// Eats the outer set of brackets.
        /// </summary>
        /// <returns>Array of indexes</returns>
        short[] GetIndexes();

        /// <summary>
        /// Gets a variable name including the % or $
        /// </summary>
        /// <returns>Name of the variable.</returns>
        string GetVariableName();
    }
}
