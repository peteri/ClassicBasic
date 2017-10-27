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
        private static Token endOfLineToken = new Token(Environment.NewLine, TokenType.ClassSeperator | TokenType.EndOfLine);

        private int? _lineNumber;

        private IToken _previousToken;

        private List<IToken> _tokens;

        private int _currentToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramLine"/> class.
        /// </summary>
        /// <param name="lineNumber">Line number.</param>
        /// <param name="tokens">Tokens in the line.</param>
        public ProgramLine(int? lineNumber, List<IToken> tokens)
        {
            _lineNumber = lineNumber;
            _tokens = tokens;
            _previousToken = null;
            _currentToken = 0;
        }

        /// <summary>
        /// Gets or sets the current token position.
        /// </summary>
        public int CurrentToken
        {
            get
            {
                return _currentToken - ((_previousToken == null) ? 0 : 1);
            }

            set
            {
                _currentToken = value;
                _previousToken = null;
            }
        }

        /// <summary>
        /// Gets the current line number.
        /// </summary>
        public int? LineNumber => _lineNumber;

        /// <summary>
        /// Gets a value indicating whether the parser has run off the end of the line.
        /// </summary>
        public bool EndOfLine
        {
            get { return CurrentToken >= _tokens.Count; }
        }

        /// <summary>
        /// Gets the next token.
        /// </summary>
        /// <returns>The next token.</returns>
        public IToken NextToken()
        {
            if (_previousToken != null)
            {
                var saved = _previousToken;
                _previousToken = null;
                return saved;
            }

            return EndOfLine ? endOfLineToken : _tokens[CurrentToken++];
        }

        /// <summary>
        /// Puts token back on the stack..
        /// </summary>
        /// <param name="token">Token to put back.</param>
        public void PushToken(IToken token)
        {
            _previousToken = (token == endOfLineToken) ? null : token;
            if (_previousToken != null)
            {
                if ((_currentToken == 0) || (_tokens[_currentToken - 1] != token))
                {
                    throw new InvalidOperationException("Token isn't the original token.");
                }
            }
        }
    }
}
