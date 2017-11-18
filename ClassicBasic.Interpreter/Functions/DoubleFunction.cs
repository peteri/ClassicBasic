// <copyright file="DoubleFunction.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for simple mathematical function that take a double and return a double.
    /// Uses a function passed in on construction as it produces cleaner child classes.
    /// </summary>
    public class DoubleFunction : Token, IFunction
    {
        private readonly Func<double, double> _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleFunction"/> class.
        /// </summary>
        /// <param name="text">Name of the function.</param>
        /// <param name="function">Function to call when executed.</param>
        public DoubleFunction(string text, Func<double, double> function)
            : base(text, TokenClass.Function)
        {
            _function = function;
        }

        /// <summary>
        /// Executes the double function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Cose of the input value</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            return new Accumulator(_function(parameters[0].ValueAsDouble()));
        }
    }
}
