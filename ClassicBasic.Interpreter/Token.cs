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

        private readonly TokenClass _tokenClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// If the text parameter starts with a letter, marked as a variable otherwise it's a number.
        /// </summary>
        /// <param name="text">Value typed in by the user.</param>
        public Token(string text)
        {
            _text = text;
            _tokenClass = char.IsLetter(text[0]) ?
                TokenClass.Variable :
                TokenClass.Number;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="text">text.</param>
        /// <param name="tokenClass">Token class.</param>
        /// <param name="tokenType">Token type.</param>
        public Token(string text, TokenClass tokenClass, TokenType tokenType)
        {
            _text = text;
            _tokenType = tokenType;
            _tokenClass = tokenClass;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="text">text.</param>
        /// <param name="tokenClass">Token class.</param>
        public Token(string text, TokenClass tokenClass)
            : this(text, tokenClass, TokenType.Unknown)
        {
        }

        /// <summary>
        /// Gets the text for the token, for built ins this is fixed, the tokeniser adds new
        /// tokens for strings, variables and numbers.
        /// </summary>
        public string Text => _text;

        /// <summary>
        /// Gets type of the token.
        /// </summary>
        public TokenClass TokenClass => _tokenClass;

        /// <summary>
        /// Gets the token is a statement this may be set to a known value or Unknown.
        /// We can find the LET command so we can fake it to allow make it optional.
        /// </summary>
        public TokenType Statement
        {
            get
            {
                return (_tokenClass == TokenClass.Statement) ?
                        _tokenType : TokenType.Unknown;
            }
        }

        /// <summary>
        /// Gets the seperator enum for the token.
        /// </summary>
        public TokenType Seperator
        {
            get
            {
                return (_tokenClass == TokenClass.Seperator) ?
                        _tokenType : TokenType.Unknown;
            }
        }

        /// <summary>
        /// Override so we can print stuff out for listings.
        /// </summary>
        /// <returns>Converted version of string.</returns>
        public override string ToString()
        {
            switch (_tokenClass)
            {
                case TokenClass.Function:
                    return " " + _text;
                case TokenClass.Statement:
                    return " " + _text + " ";
                case TokenClass.Variable:
                case TokenClass.Number:
                case TokenClass.Seperator:
                case TokenClass.Remark:
                case TokenClass.Data:
                    return _text;
                case TokenClass.String:
                    return "\"" + _text + "\"";
                default:
                    return $"Unknown class {_tokenClass}";
            }
        }
    }
}