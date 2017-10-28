// <copyright file="Fre.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the Fre function.
    /// </summary>
    public class Fre : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fre"/> class.
        /// </summary>
        public Fre()
            : base("FRE", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the Fre function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Free of the input value</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            return new Accumulator(48000.0);
        }
    }
}
