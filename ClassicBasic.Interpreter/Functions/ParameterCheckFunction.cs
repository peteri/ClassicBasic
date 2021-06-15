// <copyright file="ParameterCheckFunction.cs" company="Peter Ibbotson">
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
    public class ParameterCheckFunction : Token, IFunction
    {
        private readonly Func<double, double> _function;
        private readonly Func<double, bool> _parameterCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCheckFunction"/> class.
        /// </summary>
        /// <param name="text">Name of the function.</param>
        /// <param name="function">Function to call when executed.</param>
        /// <param name="parameterCheck">Returns true if parameter in range.</param>
        public ParameterCheckFunction(
            string text,
            Func<double, double> function,
            Func<double, bool> parameterCheck)
            : base(text, TokenClass.Function)
        {
            _function = function;
            _parameterCheck = parameterCheck;
        }

        /// <summary>
        /// Executes the double function.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        /// <returns>Cose of the input value.</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            double input = parameters[0].ValueAsDouble();
            if (!_parameterCheck(input))
            {
                throw new Exceptions.IllegalQuantityException();
            }

            return new Accumulator(_function(input));
        }
    }
}
