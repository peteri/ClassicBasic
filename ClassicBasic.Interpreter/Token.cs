// <copyright file="Token.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Base class for token.
    /// </summary>
    public class Token : IToken
    {
        private readonly string _text;

        private readonly TokenType _tokenType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// If the text parameter starts with a number, marked as a number otherwise it's a variable.
        /// </summary>
        /// <param name="text">Value typed in by the user.</param>
        public Token(string text)
        {
            _text = text;
            _tokenType = char.IsDigit(text[0]) ? TokenType.ClassNumber : TokenType.ClassVariable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="text">text.</param>
        /// <param name="seperator">Seperator identifier.</param>
        public Token(string text, TokenType seperator)
        {
            _text = text;
            _tokenType = seperator;
        }

        /// <summary>
        /// Gets the text for the token, for built ins this is fixed, the tokeniser adds new
        /// tokens for strings, variables and numbers.
        /// </summary>
        public string Text => _text;

        /// <summary>
        /// Gets type of the token.
        /// </summary>
        public TokenType TokenClass => _tokenType & TokenType.ClassMask;

        /// <summary>
        /// Gets the token is a statement this may be set to a known value or Unknown.
        /// We can find the LET command so we can fake it to allow make it optional.
        /// </summary>
        public TokenType Statement
        {
            get
            {
                return ((_tokenType & TokenType.ClassStatement) == TokenType.ClassStatement) ?
                        (_tokenType & ~TokenType.ClassStatement) : TokenType.Unknown;
            }
        }

        /// <summary>
        /// Gets the seperator enum for the token.
        /// </summary>
        public TokenType Seperator
        {
            get
            {
                return ((_tokenType & TokenType.ClassSeperator) == TokenType.ClassSeperator) ?
                        (_tokenType & ~TokenType.ClassSeperator) : TokenType.Unknown;
            }
        }

        /// <summary>
        /// Override so we can print stuff out for listings.
        /// </summary>
        /// <returns>Converted version of string.</returns>
        public override string ToString()
        {
            switch (TokenClass)
            {
                case TokenType.ClassFunction:
                    return " " + _text;
                case TokenType.ClassStatement:
                    return " " + _text + " ";
                case TokenType.ClassVariable:
                case TokenType.ClassNumber:
                case TokenType.ClassSeperator:
                case TokenType.ClassRemark:
                    return _text;
                case TokenType.ClassString:
                    return "\"" + _text + "\"";
                default:
                    return $"Unknown type {_tokenType}";
            }
        }
    }
}