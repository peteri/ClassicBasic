// <copyright file="OutOfMemoryException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Out of memory exception.
    /// </summary>
    public class OutOfMemoryException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfMemoryException"/> class.
        /// </summary>
        public OutOfMemoryException()
            : base("OUT OF MEMORY")
        {
        }
    }
}
