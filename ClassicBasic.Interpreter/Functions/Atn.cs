// <copyright file="Atn.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Atan function.
    /// </summary>
    public class Atn : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Atn"/> class.
        /// </summary>
        public Atn()
            : base("ATN", Math.Atan)
        {
        }
    }
}
