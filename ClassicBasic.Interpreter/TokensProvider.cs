// <copyright file="TokensProvider.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;

    /// <summary>
    /// Tokens provider, returns a list tokens for the interpreter.
    /// </summary>
    public class TokensProvider : ITokensProvider
    {
        private readonly List<IToken> _tokens;

        private readonly IToken[] _staticTokens =
        {
            new Token("FN", TokenClass.Statement, TokenType.Fn),
            new Token("TO", TokenClass.Statement, TokenType.To),
            new Token("TAB(", TokenClass.Statement, TokenType.Tab),
            new Token("THEN", TokenClass.Statement, TokenType.Then),
            new Token("STEP", TokenClass.Statement, TokenType.Step),
            new Token("SPC(", TokenClass.Statement, TokenType.Space),
            new Token("SYSTEM", TokenClass.Statement, TokenType.System),
            new Token("?", TokenClass.Seperator, TokenType.Print),
            new Token("+", TokenClass.Seperator, TokenType.Plus),
            new Token("-", TokenClass.Seperator, TokenType.Minus),
            new Token("/", TokenClass.Seperator, TokenType.Divide),
            new Token("=", TokenClass.Seperator, TokenType.Equal),
            new Token("^", TokenClass.Seperator, TokenType.Power),
            new Token(":", TokenClass.Seperator, TokenType.Colon),
            new Token(",", TokenClass.Seperator, TokenType.Comma),
            new Token("$", TokenClass.Seperator, TokenType.Dollar),
            new Token("%", TokenClass.Seperator, TokenType.Percent),
            new Token("<", TokenClass.Seperator, TokenType.LessThan),
            new Token("*", TokenClass.Seperator, TokenType.Multiply),
            new Token(";", TokenClass.Seperator, TokenType.Semicolon),
            new Token(">", TokenClass.Seperator, TokenType.GreaterThan),
            new Token("(", TokenClass.Seperator, TokenType.OpenBracket),
            new Token(")", TokenClass.Seperator, TokenType.CloseBracket),
            new Token("OR", TokenClass.Seperator, TokenType.Or),
            new Token("NOT", TokenClass.Seperator, TokenType.Not),
            new Token("AND", TokenClass.Seperator, TokenType.And),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TokensProvider"/> class.
        /// </summary>
        /// <param name="tokens">List of additional classes tokens, usually populated via Ioc.</param>
        public TokensProvider(IEnumerable<IToken> tokens)
        {
            _tokens = new List<IToken>();
            _tokens.AddRange(tokens);
            _tokens.AddRange(_staticTokens);
        }

        /// <summary>
        /// Gets the list of tokens.
        /// </summary>
        public List<IToken> Tokens => _tokens;
    }
}
