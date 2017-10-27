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
        public BasicException(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the error message for the user.
        /// </summary>
        public string ErrorMessage { get; }
    }
}
