﻿// <copyright file="IExpressionEvaluator.cs" company="Peter Ibbotson">
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
        /// If the next token can be evaluated as line number returns the
        /// line number. If not returns null and puts the token back.
        /// </summary>
        /// <returns>null or a line number.</returns>
        int? GetLineNumber();
    }
}
