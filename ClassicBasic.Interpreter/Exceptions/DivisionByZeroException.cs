// <copyright file="DivisionByZeroException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Exception thrown for divide by zero.
    /// </summary>
    public class DivisionByZeroException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionByZeroException"/> class.
        /// </summary>
        public DivisionByZeroException()
            : base("DIVISION BY ZERO")
        {
        }
    }
}
