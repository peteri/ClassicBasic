// <copyright file="TeletypeWithPosition.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Wrapper for the teletype that keeps a note of the current position.
    /// </summary>
    public class TeletypeWithPosition : ITeletypeWithPosition
    {
        private int _currentPosition = 0;
        private ITeletype _teletype;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeletypeWithPosition"/> class.
        /// </summary>
        /// <param name="teletype">ITeletype to wrap.</param>
        public TeletypeWithPosition(ITeletype teletype)
        {
            _teletype = teletype;
            _currentPosition = 0;
        }

        /// <summary>
        /// Prints a new line.
        /// </summary>
        public void NewLine()
        {
            _teletype.Write(Environment.NewLine);
            _currentPosition = 0;
        }

        /// <summary>
        /// Moves on to the next comma position.
        /// </summary>
        public void NextComma()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prints spaces.
        /// </summary>
        /// <param name="count">Position to move to.</param>
        public void Space(short count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves across to a tab position.
        /// </summary>
        /// <param name="count">Position to move to.</param>
        public void Tab(short count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes out a text to a console.
        /// </summary>
        /// <param name="text">text to write out.</param>
        public void Write(string text)
        {
            _teletype.Write(text);
            _currentPosition += text.Length;
        }
    }
}
