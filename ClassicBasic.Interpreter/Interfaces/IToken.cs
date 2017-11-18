// <copyright file="IToken.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Interface to the tokens.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Gets the text for the token, for built ins this is fixed, the tokeniser adds new
        /// tokens for strings, variables and numbers.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets class of the token.
        /// </summary>
        TokenClass TokenClass { get; }

        /// <summary>
        /// Gets if the token is a statement this may be set to a known value or Unknown.
        /// We can find the LET command so we can fake it to allow make it optional.
        /// </summary>
        TokenType Statement { get; }

        /// <summary>
        /// Gets the seperator enum for the token.
        /// </summary>
        TokenType Seperator { get; }
    }
}