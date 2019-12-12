// <copyright file="Rnd.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Rnd function.
    /// </summary>
    public class Rnd : Token, IFunction
    {
        private Random _random;
        private double _lastResult = 0.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rnd"/> class.
        /// </summary>
        public Rnd()
            : base("RND", TokenClass.Function)
        {
            _random = new Random();
            _lastResult = _random.NextDouble();
        }

        /// <summary>
        /// Executes the Rnd function.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        /// <returns>Rnde of the input value.</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            double param = parameters[0].ValueAsDouble();
            if (param < 0.0)
            {
                _random = new Random(param.GetHashCode());
                _lastResult = _random.NextDouble();
            }

            if (param > 0.0)
            {
                _lastResult = _random.NextDouble();
            }

            return new Accumulator(_lastResult);
        }
    }
}
