// <copyright file="OutOfDataException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Thrown when we run out data for the READ command.
    /// </summary>
    public class OutOfDataException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfDataException"/> class.
        /// </summary>
        public OutOfDataException()
            : base("OUT OF DATA")
        {
        }
    }
}
