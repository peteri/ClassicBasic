// <copyright file="IllegalDeferredException.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Exceptions
{
    /// <summary>
    /// IllegalDeferredException, thrown by the edit command as it's only valid
    /// in immediate mode.
    /// </summary>
    public class IllegalDeferredException : BasicException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalDeferredException"/> class.
        /// </summary>
        public IllegalDeferredException()
            : base("ILLEGAL DEFERRED", 104)
        {
        }
    }
}
