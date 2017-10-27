// <copyright file="RedimensionedArrayException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Redimensioned array error exception.
    /// </summary>
    public class RedimensionedArrayException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedimensionedArrayException"/> class.
        /// </summary>
        public RedimensionedArrayException()
            : base("REDIM\'D ARRAY")
        {
        }
    }
}
