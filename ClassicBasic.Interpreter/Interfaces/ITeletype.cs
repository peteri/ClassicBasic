// <copyright file="ITeletype.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Defines the glass teletype interface.
    /// </summary>
    public interface ITeletype
    {
        /// <summary>
        /// Event handler for Ctrl-C
        /// </summary>
        event ConsoleCancelEventHandler CancelEventHandler;

        /// <summary>
        /// Write text to the glass teletype (aka console)
        /// </summary>
        /// <param name="output">Text to write.</param>
        void Write(string output);

        /// <summary>
        /// Read a string from the keyboard.
        /// </summary>
        /// <returns>String user typed in.</returns>
        string Read();
    }
}
