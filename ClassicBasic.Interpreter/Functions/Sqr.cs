// <copyright file="Sqr.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Sqr function.
    /// </summary>
    public class Sqr : PositiveFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sqr"/> class.
        /// </summary>
        public Sqr()
            : base("SQR", Math.Sqrt)
        {
        }
    }
}
