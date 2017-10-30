// <copyright file="Asc.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the Asc function.
    /// </summary>
    public class Asc : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asc"/> class.
        /// </summary>
        public Asc()
            : base("ASC", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the Asc function.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        /// <returns>UTF-16 character of the first character of the input value.</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            string paramAsString = parameters[0].ValueAsString();
            if (paramAsString.Length == 0)
            {
                throw new Exceptions.IllegalQuantityException();
            }

            int returnValue = paramAsString[0];
            if (returnValue > short.MaxValue)
            {
                returnValue = returnValue - 0x10000;
            }

            return new Accumulator((double)returnValue);
        }
    }
}
