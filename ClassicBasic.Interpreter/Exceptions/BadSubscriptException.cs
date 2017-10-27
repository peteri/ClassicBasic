// <copyright file="BadSubscriptException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Bad array subscript exception.
    /// </summary>
    public class BadSubscriptException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadSubscriptException"/> class.
        /// </summary>
        public BadSubscriptException()
            : base("BAD SUBSCRIPT")
        {
        }
    }
}
