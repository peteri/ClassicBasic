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
            var token = new Token("LET", TokenType.ClassStatement | TokenType.Let);
            Assert.AreEqual(TokenType.Let, token.Statement);
            Assert.AreEqual(TokenType.ClassStatement, token.TokenClass);
        }

        /// <summary>
        /// Token.Statement returns unknown for the TokenType when the TokenClass is a not a Statement.
        /// </summary>
        [TestMethod]
        public void TokenStatementReturnsUnknownForSeperators()
        {
            var token = new Token("*", TokenType.ClassSeperator | TokenType.Multiply);
            Assert.AreEqual(TokenType.Unknown, token.Statement);
            Assert.AreNotEqual(TokenType.ClassStatement, token.TokenClass);
        }

        /// <summary>
        /// Token.Seperator returns back the TokenType when the TokenClass is a Seperator.
        /// </summary>
        [TestMethod]
        public void TokenSeperatorReturnsIdentityForSeperators()
        {
            var token = new Token("*", TokenType.ClassSeperator | TokenType.Multiply);
            Assert.AreEqual(TokenType.Multiply, token.Seperator);
            Assert.AreEqual(TokenType.ClassSeperator, token.TokenClass);
        }

        /// <summary>
        /// Token.Seperator returns unknown for the TokenType when the TokenClass is a not a Seperator.
        /// </summary>
        [TestMethod]
        public void TokenSeperatorReturnsUnknownForStatements()
        {
            var token = new Token("LET", TokenType.ClassStatement | TokenType.Let);
            Assert.AreEqual(TokenType.Unknown, token.Seperator);
            Assert.AreNotEqual(TokenType.ClassSeperator, token.TokenClass);
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
            Assert.AreEqual(TokenType.ClassNumber, token.TokenClass);
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
            Assert.AreEqual(TokenType.ClassNumber, token.TokenClass);
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
            Assert.AreEqual(TokenType.ClassVariable, token.TokenClass);
        }

        /// <summary>
        /// Tests ToString.
        /// </summary>
        /// <param name="text">Value.</param>
        /// <param name="type">Type of the token.</param>
        /// <param name="textOut">To string output.</param>
        [DataTestMethod]
        [DataRow("A", TokenType.ClassFunction, " A")]
        [DataRow("B", TokenType.ClassNumber, "B")]
        [DataRow("C", TokenType.ClassRemark, "C")]
        [DataRow("D", TokenType.ClassSeperator, "D")]
        [DataRow("E", TokenType.ClassStatement, " E ")]
        [DataRow("F", TokenType.ClassString, "\"F\"")]
        [DataRow("G", TokenType.ClassVariable, "G")]
        [DataRow("H", TokenType.Unknown, "Unknown type Unknown")]
        public void TokenToStringTest(string text, TokenType type, string textOut)
        {
            var token = new Token(text, type);
            Assert.AreEqual(textOut, token.ToString());
        }
    }
}
