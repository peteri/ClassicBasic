// <copyright file="Log.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Log function.
    /// </summary>
    public class Log : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
            : base("LOG", Math.Log)
        {
        }
    }
}
