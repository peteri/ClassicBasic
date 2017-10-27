// <copyright file="StringToLongException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// String too long exception
    /// </summary>
    public class StringToLongException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringToLongException"/> class.
        /// </summary>
        public StringToLongException()
            : base("STRING TOO LONG")
        {
        }
    }
}
