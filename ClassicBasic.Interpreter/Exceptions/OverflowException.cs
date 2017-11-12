// <copyright file="OverflowException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Numeric overflow exception.
    /// </summary>
    public class OverflowException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverflowException"/> class.
        /// </summary>
        public OverflowException()
           : base("OVERFLOW", 6)
        {
        }
    }
}
