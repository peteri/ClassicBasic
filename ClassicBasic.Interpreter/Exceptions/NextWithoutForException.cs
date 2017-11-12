// <copyright file="NextWithoutForException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Next without for exception.
    /// </summary>
    public class NextWithoutForException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NextWithoutForException"/> class.
        /// </summary>
        public NextWithoutForException()
            : base("NEXT WITHOUT FOR", 1)
        {
        }
    }
}
