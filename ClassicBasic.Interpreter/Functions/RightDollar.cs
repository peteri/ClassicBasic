// <copyright file="RightDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the RIGHT$ function.
    /// </summary>
    public class RightDollar : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightDollar"/> class.
        /// </summary>
        public RightDollar()
            : base("RIGHT$", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the RIGHT$ function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>left string of the parameters</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 2)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var returnValue = parameters[0].ValueAsString();
            int count = (int)parameters[1].ValueAsDouble();
            if ((count < 0) || (count > 255))
            {
                throw new Exceptions.IllegalQuantityException();
            }
#warning FixMe
            return new Accumulator(returnValue.Substring(0, count));
        }
    }
}
