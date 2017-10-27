// <copyright file="ReturnWithoutGosubException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Exception thrown when the user runs out of gosub stack.
    /// </summary>
    public class ReturnWithoutGosubException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnWithoutGosubException"/> class.
        /// </summary>
        public ReturnWithoutGosubException()
            : base("RETURN WITHOUT GOSUB")
        {
        }
    }
}
