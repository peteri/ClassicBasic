// <copyright file="UnableToEditException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Unable to edit exception.
    /// </summary>
    public class UnableToEditException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnableToEditException"/> class.
        /// </summary>
        public UnableToEditException()
            : base("EDIT NOT AVAILABLE", 103)
        {
        }
    }
}
