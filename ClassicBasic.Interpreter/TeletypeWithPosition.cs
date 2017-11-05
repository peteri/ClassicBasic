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
        private const int CommaCellWidth = 14;
        private int _currentPosition = 1;
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
            // Originally commas are split in to 14 character cells
            // which makes sense where width is 72.
            var newPosition = ((_currentPosition / CommaCellWidth) + 1) * CommaCellWidth;
            if ((newPosition + CommaCellWidth) >= _teletype.Width)
            {
                NewLine();
            }
            else
            {
                Tab((short)(newPosition + 1));
            }
        }

        /// <summary>
        /// Prints spaces.
        /// </summary>
        /// <param name="count">Position to move to.</param>
        public void Space(short count)
        {
            if (count < 0 || count > 255)
            {
                throw new Exceptions.IllegalQuantityException();
            }

            Write(new string(' ', count));
        }

        /// <summary>
        /// Moves across to a tab position left most position is 1.
        /// </summary>
        /// <param name="count">Position to move to.</param>
        public void Tab(short count)
        {
            if (count < 0 || count > 255)
            {
                throw new Exceptions.IllegalQuantityException();
            }

            while (count > _teletype.Width)
            {
                count -= _teletype.Width;
                NewLine();
            }

            count--;
            var numberOfSpaces = count - _currentPosition;
            if (numberOfSpaces > 0)
            {
                Write(new string(' ', numberOfSpaces));
            }
        }

        /// <summary>
        /// Writes out a text to a console.
        /// </summary>
        /// <param name="text">text to write out.</param>
        public void Write(string text)
        {
            _teletype.Write(text);
            _currentPosition = (_currentPosition + text.Length) % _teletype.Width;
        }

        /// <summary>
        /// Read a string from the keyboard, also resets the current horizontal position.
        /// </summary>
        /// <returns>String user typed in.</returns>
        public string Read()
        {
            var input = _teletype.Read();
            _currentPosition = 0;
            return input;
        }

        /// <summary>
        /// Returns the current position on the line numbered from 1.
        /// </summary>
        /// <returns>Current position.</returns>
        public short Position()
        {
            return (short)(_currentPosition + 1);
        }

        /// <summary>
        /// Read a character from the keyboard.
        /// </summary>
        /// <returns>Character user typed in.</returns>
        public char ReadChar()
        {
            return _teletype.ReadChar();
        }
    }
}
