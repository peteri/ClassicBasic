// <copyright file="CharDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the CHR$ function.
    /// </summary>
    public class CharDollar : Token, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharDollar"/> class.
        /// </summary>
        public CharDollar()
            : base("CHR$", TokenType.ClassFunction)
        {
        }

        /// <summary>
        /// Executes the Left$ function.
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

            return new Accumulator(returnValue.Substring(0, count));
        }
    }
}
