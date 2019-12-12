// <copyright file="IExecutor.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Executes the current line or program.
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Executes the current line or program.
        /// </summary>
        /// <returns>true if the user type SYSTEM.</returns>
        bool ExecuteLine();
    }
}