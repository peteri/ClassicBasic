// <copyright file="IllegalQuantityException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Represents an out of range value.
    /// </summary>
    public class IllegalQuantityException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalQuantityException"/> class.
        /// </summary>
        public IllegalQuantityException()
            : base("ILLEGAL QUANTITY", 5)
        {
        }
    }
}
