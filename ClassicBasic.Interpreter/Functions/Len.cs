// <copyright file="Len.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Len function.
    /// </summary>
    public class Len : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Len"/> class.
        /// </summary>
        public Len()
            : base("LEN", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the Len function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Length of the input value</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var returnValue = parameters[0].ValueAsString();
            return new Accumulator((double)returnValue.Length);
        }
    }
}
