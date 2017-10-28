// <copyright file="Abs.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Abs function.
    /// </summary>
    public class Abs : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Abs"/> class.
        /// </summary>
        public Abs()
            : base("ABS", Math.Abs)
        {
        }
    }
}
