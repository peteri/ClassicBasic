// <copyright file="CantContinueException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Can't continue exception, thrown when we can't execute the CONT command
    /// </summary>
    public class CantContinueException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CantContinueException"/> class.
        /// </summary>
        public CantContinueException()
            : base("CAN\'T CONTINUE", 17)
        {
        }
    }
}
