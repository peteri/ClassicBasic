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
        private List<IToken> _tokens;

        private IToken[] _staticTokens =
        {
            new Token("FN", TokenType.ClassStatement | TokenType.Fn),
            new Token("TO", TokenType.ClassStatement | TokenType.To),
            new Token("TAB(", TokenType.ClassStatement | TokenType.Tab),
            new Token("THEN", TokenType.ClassStatement | TokenType.Then),
            new Token("STEP", TokenType.ClassStatement | TokenType.Step),
            new Token("SPC(", TokenType.ClassStatement | TokenType.Space),
            new Token("SYSTEM", TokenType.ClassStatement | TokenType.System),
            new Token("?", TokenType.ClassSeperator | TokenType.Print),
            new Token("+", TokenType.ClassSeperator | TokenType.Plus),
            new Token("-", TokenType.ClassSeperator | TokenType.Minus),
            new Token("/", TokenType.ClassSeperator | TokenType.Divide),
            new Token("=", TokenType.ClassSeperator | TokenType.Equal),
            new Token("^", TokenType.ClassSeperator | TokenType.Power),
            new Token(":", TokenType.ClassSeperator | TokenType.Colon),
            new Token(",", TokenType.ClassSeperator | TokenType.Comma),
            new Token("$", TokenType.ClassSeperator | TokenType.Dollar),
            new Token("%", TokenType.ClassSeperator | TokenType.Percent),
            new Token("<", TokenType.ClassSeperator | TokenType.LessThan),
            new Token("*", TokenType.ClassSeperator | TokenType.Multiply),
            new Token(";", TokenType.ClassSeperator | TokenType.Semicolon),
            new Token(">", TokenType.ClassSeperator | TokenType.GreaterThan),
            new Token("(", TokenType.ClassSeperator | TokenType.OpenBracket),
            new Token(")", TokenType.ClassSeperator | TokenType.CloseBracket),
            new Token("OR", TokenType.ClassSeperator | TokenType.Or),
            new Token("NOT", TokenType.ClassSeperator | TokenType.Not),
            new Token("AND", TokenType.ClassSeperator | TokenType.And)
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
