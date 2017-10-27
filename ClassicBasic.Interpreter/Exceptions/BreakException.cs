// <copyright file="BreakException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Exception used to signal the user has either hit break OR
    /// used the stop command
    /// </summary>
    public class BreakException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BreakException"/> class.
        /// </summary>
        public BreakException()
            : this("BREAK")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakException"/> class.
        /// </summary>
        /// <param name="message">Message for the user.</param>
        public BreakException(string message)
            : base(message)
        {
        }
    }
}
