// <copyright file="Sgn.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Sgn function.
    /// </summary>
    public class Sgn : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sgn"/> class.
        /// </summary>
        public Sgn()
            : base("SGN", (x) => (double)Math.Sign(x))
        {
        }
    }
}
