// <copyright file="LeftDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Left$ function.
    /// </summary>
    public class LeftDollar : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeftDollar"/> class.
        /// </summary>
        public LeftDollar()
            : base("LEFT$", TokenClass.Function)
        {
        }

        /// <summary>
        /// Executes the LEFT$ function.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        /// <returns>left string of the parameters.</returns>
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

            count = Math.Min(count, returnValue.Length);
            return new Accumulator(returnValue.Substring(0, count));
        }
    }
}
