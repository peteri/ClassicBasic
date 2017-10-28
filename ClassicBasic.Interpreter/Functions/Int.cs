// <copyright file="Int.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Functions
{
    using System;

    /// <summary>
    /// Implements the Int function.
    /// </summary>
    public class Int : DoubleFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Int"/> class.
        /// </summary>
        public Int()
            : base("INT", Math.Floor)
        {
        }
    }
}
