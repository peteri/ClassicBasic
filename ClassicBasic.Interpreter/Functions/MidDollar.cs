// <copyright file="MidDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Left$ function.
    /// </summary>
    public class MidDollar : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MidDollar"/> class.
        /// </summary>
        public MidDollar()
            : base("MID$", TokenClass.Function)
        {
        }

        /// <summary>
        /// Executes the MID$ function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Mid string of the parameters</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 2 && parameters.Count != 3)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            string returnValue = parameters[0].ValueAsString();
            int start = (int)parameters[1].ValueAsShort();
            int length = (parameters.Count == 2) ? 255 : parameters[2].ValueAsShort();

            if ((start <= 0) || (start > 255))
            {
                throw new Exceptions.IllegalQuantityException();
            }

            if ((length < 0) || (length > 255))
            {
                throw new Exceptions.IllegalQuantityException();
            }

            start--;
            start = Math.Min(start, returnValue.Length);
            length = Math.Min(length, returnValue.Length - start);
            return new Accumulator(returnValue.Substring(start, length));
        }
    }
}
