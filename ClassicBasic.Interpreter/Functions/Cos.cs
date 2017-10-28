// <copyright file="Cos.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Cos function.
    /// </summary>
    public class Cos : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cos"/> class.
        /// </summary>
        public Cos()
            : base("COS", Math.Cos)
        {
        }
    }
}
