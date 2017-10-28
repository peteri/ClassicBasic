// <copyright file="ITeletypeWithPosition.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Teletype interface which keeps track of the current horizontal position.
    /// </summary>
    public interface ITeletypeWithPosition
    {
        /// <summary>
        /// Moves across to a tab position.
        /// </summary>
        /// <param name="count">Position to move to.</param>
        void Tab(short count);

        /// <summary>
        /// Prints spaces.
        /// </summary>
        /// <param name="count">Position to move to.</param>
        void Space(short count);

        /// <summary>
        /// Moves on to the next comma position.
        /// </summary>
        void NextComma();

        /// <summary>
        /// Prints a new line.
        /// </summary>
        void NewLine();

        /// <summary>
        /// Writes out a text to a console.
        /// </summary>
        /// <param name="text">text to write out.</param>
        void Write(string text);

        /// <summary>
        /// Read a string from the keyboard.
        /// </summary>
        /// <returns>String user typed in.</returns>
        string Read();

        /// <summary>
        /// Returns the current position on the line numbered from 0.
        /// </summary>
        /// <returns>Current position.</returns>
        short Position();
    }
}
