// <copyright file="EndException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Exception used by the end command. Since the message is empty,
    /// it doesn't display to the user.
    /// </summary>
    public class EndException : BreakException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndException"/> class.
        /// </summary>
        public EndException()
            : base(string.Empty)
        {
        }
    }
}
