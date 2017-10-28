// <copyright file="Pos.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Interfaces;

    /// <summary>
    /// Implements the Pos function.
    /// </summary>
    public class Pos : Token, IFunction
    {
        private readonly ITeletypeWithPosition _teletypeWithPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pos"/> class.
        /// </summary>
        /// <param name="teletypeWithPosition">Teletype to get the cursor position for.</param>
        public Pos(ITeletypeWithPosition teletypeWithPosition)
            : base("POS", TokenType.ClassFunction)
        {
            _teletypeWithPosition = teletypeWithPosition;
        }

        /// <summary>
        /// Executes the Pos function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Current horizontal position</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            return new Accumulator((double)_teletypeWithPosition.Position());
        }
    }
}
