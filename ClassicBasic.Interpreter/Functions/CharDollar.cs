// <copyright file="CharDollar.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System.Collections.Generic;

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
        /// Executes the CHR$ function.
        /// </summary>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Converts the parameter to</returns>
        public Accumulator Execute(IList<Accumulator> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            int character = (int)parameters[0].ValueAsShort();
            if (character < 0)
            {
                character += 0x10000;
            }

            return new Accumulator(new string((char)character, 1));
        }
    }
}
