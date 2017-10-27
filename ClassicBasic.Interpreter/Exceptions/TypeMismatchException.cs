// <copyright file="TypeMismatchException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Type mismatch error.
    /// </summary>
    public class TypeMismatchException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMismatchException"/> class.
        /// </summary>
        public TypeMismatchException()
            : base("TYPE MISMATCH")
        {
        }
    }
}
