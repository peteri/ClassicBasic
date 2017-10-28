// <copyright file="Tan.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Tan function.
    /// </summary>
    public class Tan : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tan"/> class.
        /// </summary>
        public Tan()
            : base("TAN", Math.Tan)
        {
        }
    }
}
