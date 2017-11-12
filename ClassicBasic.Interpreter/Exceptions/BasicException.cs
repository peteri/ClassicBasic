// <copyright file="BasicException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    using System;

    /// <summary>
    /// Base class for exceptions thrown by the basic interpreter.
    /// </summary>
    public class BasicException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicException"/> class.
        /// </summary>
        /// <param name="errorMessage">Message text to display to the user.</param>
        /// <param name="errorCode">Error code.</param>
        public BasicException(string errorMessage, int errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error message for the user.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the error code for an exception.
        /// </summary>
        public int ErrorCode { get; }
    }
}
