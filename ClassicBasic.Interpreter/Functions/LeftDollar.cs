// <copyright file="LeftDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the Left$ function.
    /// </summary>
    public class LeftDollar : Token, IFunction
    {
        private readonly ITeletypeWithPosition _teletypeWithPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeftDollar"/> class.
        /// </summary>
        /// <param name="teletypeWithPosition">Teletype to get the cursor position for.</param>
        public LeftDollar(ITeletypeWithPosition teletypeWithPosition)
            : base("LEFT$", TokenType.ClassFunction)
        {
            _teletypeWithPosition = teletypeWithPosition;
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
