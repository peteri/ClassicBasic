// <copyright file="Exp.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Exp function.
    /// </summary>
    public class Exp : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Exp"/> class.
        /// </summary>
        public Exp()
            : base("EXP", Math.Exp)
        {
        }
    }
}
