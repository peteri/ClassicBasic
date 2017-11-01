// <copyright file="ProgramLine.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a program line.
    /// </summary>
    public class ProgramLine
    {
        private static readonly Token EndOfLineToken = new Token(Environment.NewLine, TokenType.ClassSeperator | TokenType.EndOfLine);

        private readonly List<IToken> _tokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramLine"/> class.
        /// </summary>
        /// <param name="lineNumber">Line number.</param>
        /// <param name="tokens">Tokens in the line.</param>
        public ProgramLine(int? lineNumber, List<IToken> tokens)
        {
            LineNumber = lineNumber;
            _tokens = tokens;
        }

        /// <summary>
        /// Gets or sets the current token position.
        /// </summary>
        public int CurrentToken { get; set; }

        /// <summary>
        /// Gets the current line number.
        /// </summary>
        public int? LineNumber { get; }

        /// <summary>
        /// Gets a value indicating whether the parser has run off the end of the line.
        /// </summary>
        public bool EndOfLine => CurrentToken >= _tokens.Count;

        /// <summary>
        /// Gets the next token.
        /// </summary>
        /// <returns>The next token.</returns>
        public IToken NextToken()
        {
            return EndOfLine ? EndOfLineToken : _tokens[CurrentToken++];
        }

        /// <summary>
        /// Puts token back on the stack..
        /// </summary>
        /// <param name="token">Token to put back.</param>
        public void PushToken(IToken token)
        {
            if (token != EndOfLineToken)
            {
                if ((CurrentToken == 0) || (_tokens[CurrentToken - 1] != token))
                {
                    throw new InvalidOperationException("Token isn't the original token.");
                }

               CurrentToken--;
            }
        }
    }
}
