// <copyright file="SyntaxErrorException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Exception thrown when there is a syntax error.
    /// </summary>
    public class SyntaxErrorException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxErrorException"/> class.
        /// </summary>
        public SyntaxErrorException()
            : base("SYNTAX", 2)
        {
        }
    }
}
