// <copyright file="ITokensProvider.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;

    /// <summary>
    /// Tokens provider, returns a list tokens for the interpreter.
    /// </summary>
    public interface ITokensProvider
    {
        /// <summary>
        /// Gets the list of tokens.
        /// </summary>
        List<IToken> Tokens { get; }
    }
}