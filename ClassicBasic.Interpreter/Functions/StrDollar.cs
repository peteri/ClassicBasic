// <copyright file="StrDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the STR$ function.
    /// </summary>
    public class StrDollar : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrDollar"/> class.
        /// </summary>
        public StrDollar()
            : base("STR$", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the STR$ function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>String value of the first parameter.</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var returnValue = parameters[0].ValueAsDouble();
            return new Accumulator(returnValue.ToString());
        }
    }
}
