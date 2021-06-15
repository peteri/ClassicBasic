// <copyright file="Val.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Val function.
    /// </summary>
    public class Val : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Val"/> class.
        /// </summary>
        public Val()
            : base("VAL", TokenClass.Function)
        {
        }

        /// <summary>
        /// Executes the VAL function.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        /// <returns>VAL of the input value.</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            string stringToParse = parameters[0].ValueAsString();

            if (!double.TryParse(stringToParse, out double returnValue))
            {
                return new Accumulator(0.0);
            }

            return new Accumulator(returnValue);
        }
    }
}
