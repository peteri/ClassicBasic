// <copyright file="Val.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the Val function.
    /// </summary>
    public class Val : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Val"/> class.
        /// </summary>
        public Val()
            : base("VAL", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the Val function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Vale of the input value</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }
#warning FixMe
            return new Accumulator(48000.0);
        }
    }
}
