// <copyright file="RightDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the RIGHT$ function.
    /// </summary>
    public class RightDollar : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightDollar"/> class.
        /// </summary>
        public RightDollar()
            : base("RIGHT$", TokenClass.Function)
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
            int count = parameters[1].ValueAsShort();
            if ((count <= 0) || (count > 255))
            {
                throw new Exceptions.IllegalQuantityException();
            }

            int start = Math.Max(0, returnValue.Length - count);
            count = returnValue.Length - start;
            return new Accumulator(returnValue.Substring(start, count));
        }
    }
}
