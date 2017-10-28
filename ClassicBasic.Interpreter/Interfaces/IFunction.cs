// <copyright file="IFunction.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a function.
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Executes a function.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        /// <returns>Accumulator with the function result.</returns>
        Accumulator Execute(IList<Accumulator> parameters);
    }
}
