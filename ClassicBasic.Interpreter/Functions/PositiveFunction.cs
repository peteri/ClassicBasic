// <copyright file="DoubleFunction.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for simple mathematical function that take a double needing
    /// a positive parameter and return a double.
    /// Uses a function passed in on construction as it produces cleaner child classes.
    /// </summary>
    public class PositiveFunction : Token, IFunction
    {
        private readonly Func<double, double> _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositiveFunction"/> class.
        /// </summary>
        /// <param name="text">Name of the function.</param>
        /// <param name="function">Function to call when executed.</param>
        public PositiveFunction(string text, Func<double, double> function)
            : base(text, TokenType.ClassFunction)
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

            double input = parameters[0].ValueAsDouble();
            if (input < 0.0)
            {
                throw new Exceptions.IllegalQuantityException();
            }

            return new Accumulator(_function(input));
        }
    }
}
