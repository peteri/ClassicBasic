// <copyright file="IDataStatementReader.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Interface to the data statement reader.
    /// </summary>
    public interface IDataStatementReader
    {
        /// <summary>
        /// Gets the shared read input parser.
        /// </summary>
        ReadInputParser ReadInputParser { get; }

        /// <summary>
        /// Implements moving the current data pointer to a new line number
        /// </summary>
        /// <param name="lineNumber">line number to move to, null moves to beginning of program.</param>
        void RestoreToLineNumber(int? lineNumber);
    }
}