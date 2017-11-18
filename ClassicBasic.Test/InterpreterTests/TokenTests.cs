// <copyright file="TokenTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the token class.
    /// </summary>
    [TestClass]
    public class TokenTests
    {
        /// <summary>
        /// Token.Statement returns back the TokenType when the TokenClass is a Statement.
        /// </summary>
        [TestMethod]
        public void TokenStatementReturnsIdentityForStatements()
        {
            var token = new Token("LET", TokenClass.Statement, TokenType.Let);
            Assert.AreEqual(TokenType.Let, token.Statement);
            Assert.AreEqual(TokenClass.Statement, token.TokenClass);
        }

        /// <summary>
        /// Token.Statement returns unknown for the TokenType when the TokenClass is a not a Statement.
        /// </summary>
        [TestMethod]
        public void TokenStatementReturnsUnknownForSeperators()
        {
            var token = new Token("*", TokenClass.Seperator, TokenType.Multiply);
            Assert.AreEqual(TokenType.Unknown, token.Statement);
            Assert.AreNotEqual(TokenClass.Statement, token.TokenClass);
        }

        /// <summary>
        /// Token.Seperator returns back the TokenType when the TokenClass is a Seperator.
        /// </summary>
        [TestMethod]
        public void TokenSeperatorReturnsIdentityForSeperators()
        {
            var token = new Token("*", TokenClass.Seperator, TokenType.Multiply);
            Assert.AreEqual(TokenType.Multiply, token.Seperator);
            Assert.AreEqual(TokenClass.Seperator, token.TokenClass);
        }

        /// <summary>
        /// Token.Seperator returns unknown for the TokenType when the TokenClass is a not a Seperator.
        /// </summary>
        [TestMethod]
        public void TokenSeperatorReturnsUnknownForStatements()
        {
            var token = new Token("LET", TokenClass.Statement, TokenType.Let);
            Assert.AreEqual(TokenType.Unknown, token.Seperator);
            Assert.AreNotEqual(TokenClass.Seperator, token.TokenClass);
        }

        /// <summary>
        /// Token created with text and the string begins with a non-letter is created as number.
        /// </summary>
        [TestMethod]
        public void TokenCreatedWithTextBeginningWithNotALetterIsNumber()
        {
            var token = new Token("0a1");
            Assert.AreEqual("0a1", token.Text);
            Assert.AreEqual(TokenType.Unknown, token.Seperator);
            Assert.AreEqual(TokenType.Unknown, token.Statement);
            Assert.AreEqual(TokenClass.Number, token.TokenClass);
        }

        /// <summary>
        /// Token created with text and the string begins with a period is created as number.
        /// </summary>
        [TestMethod]
        public void TokenCreatedWithTextBeginningWithAPeriodIsNumber()
        {
            var token = new Token(".0a1");
            Assert.AreEqual(".0a1", token.Text);
            Assert.AreEqual(TokenType.Unknown, token.Seperator);
            Assert.AreEqual(TokenType.Unknown, token.Statement);
            Assert.AreEqual(TokenClass.Number, token.TokenClass);
        }

        /// <summary>
        /// Token created with text and the string begins with a letter is created as variable.
        /// </summary>
        [TestMethod]
        public void TokenCreatedWithTextBeginningWithALetterIsVariable()
        {
            var token = new Token("a0a1");
            Assert.AreEqual("a0a1", token.Text);
            Assert.AreEqual(TokenType.Unknown, token.Seperator);
            Assert.AreEqual(TokenType.Unknown, token.Statement);
            Assert.AreEqual(TokenClass.Variable, token.TokenClass);
        }

        /// <summary>
        /// Tests ToString.
        /// </summary>
        /// <param name="text">Value.</param>
        /// <param name="tokenClass">Class of the token.</param>
        /// <param name="textOut">To string output.</param>
        [DataTestMethod]
        [DataRow("A", TokenClass.Function, " A")]
        [DataRow("B", TokenClass.Number, "B")]
        [DataRow("C", TokenClass.Remark, "C")]
        [DataRow("D", TokenClass.Seperator, "D")]
        [DataRow("E", TokenClass.Statement, " E ")]
        [DataRow("F", TokenClass.String, "\"F\"")]
        [DataRow("G", TokenClass.Variable, "G")]
        [DataRow("H", TokenClass.Data, "H")]
        [DataRow("I", TokenType.Unknown, "Unknown class Unknown")]
        public void TokenToStringTest(string text, TokenClass tokenClass, string textOut)
        {
            var token = new Token(text, tokenClass);
            Assert.AreEqual(textOut, token.ToString());
        }
    }
}