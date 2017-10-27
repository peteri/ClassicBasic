// <copyright file="IllegalDirectException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// IllegalDirectException, thrown by the def fn command as it's only valid
    /// in program code.
    /// </summary>
    public class IllegalDirectException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalDirectException"/> class.
        /// </summary>
        public IllegalDirectException()
            : base("ILLEGAL DIRECT")
        {
        }
    }
}
