// <copyright file="UndefinedStatementException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Undefined statement exception, thrown when a line number can't be found.
    /// </summary>
    public class UndefinedStatementException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndefinedStatementException"/> class.
        /// </summary>
        public UndefinedStatementException()
            : base("UNDEF\'D STATEMENT", 8)
        {
        }
    }
}
