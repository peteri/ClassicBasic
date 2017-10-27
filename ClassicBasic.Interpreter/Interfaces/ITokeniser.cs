// <copyright file="ITokeniser.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Used to tokenise a line.
    /// </summary>
    public interface ITokeniser
    {
        /// <summary>
        /// Tokenises a line of text.
        /// </summary>
        /// <param name="command">Command typed by the user.</param>
        /// <returns>List of parsed tokens.</returns>
        ProgramLine Tokenise(string command);
    }
}
