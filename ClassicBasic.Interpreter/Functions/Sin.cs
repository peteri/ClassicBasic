// <copyright file="Sin.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Sin function.
    /// </summary>
    public class Sin : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sin"/> class.
        /// </summary>
        public Sin()
            : base("SIN", Math.Sin)
        {
        }
    }
}
