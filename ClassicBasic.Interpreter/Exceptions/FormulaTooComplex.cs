// <copyright file="FormulaTooComplex.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// Formula to complex exception, used if we nest too deeply (64 levels).
    /// </summary>
    public class FormulaTooComplex : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaTooComplex"/> class.
        /// </summary>
        public FormulaTooComplex()
            : base("FORMULA TOO COMPLEX", 16)
        {
        }
    }
}
