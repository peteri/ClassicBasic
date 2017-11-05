// <copyright file="UndefinedFunctionException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Undefined function exception, thrown when a function name can't be found.
    /// </summary>
    public class UndefinedFunctionException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndefinedFunctionException"/> class.
        /// </summary>
        public UndefinedFunctionException()
            : base("UNDEF\'D FUNCTION")
        {
        }
    }
}