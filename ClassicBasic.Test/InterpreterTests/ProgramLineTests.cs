// <copyright file="ProgramLineTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System;
    using System.Collections.Generic;
    using Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Program line class.
    /// </summary>
    [TestClass]
    public class ProgramLineTests
    {
        private List<IToken> _tokens = new List<IToken>
        {
            new Token("ONE", TokenClass.Variable),
            new Token("TWO", TokenClass.Variable),
            new Token("THREE", TokenClass.Variable)
        };

        /// <summary>
        /// Test that we get the tokens.
        /// </summary>
        [TestMethod]
        public void ProgramLineReturnsThreeTokens()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);

            Assert.AreEqual("ONE", programLine.NextToken().Text);
            Assert.AreEqual("TWO", programLine.NextToken().Text);
            Assert.AreEqual("THREE", programLine.NextToken().Text);
        }

        /// <summary>
        /// Test End of line returns true for empty list.
        /// </summary>
        [TestMethod]
        public void ProgramLineEndOfLineIsTrueForEmptyList()
        {
            ProgramLine programLine = new ProgramLine(30, new List<IToken>());
            Assert.IsTrue(programLine.EndOfLine);
        }

        /// <summary>
        /// Test End of line returns false for non empty list.
        /// </summary>
        [TestMethod]
        public void ProgramLineEndOfLineIsFalseForNonEmptyList()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            Assert.IsFalse(programLine.EndOfLine);
        }

        /// <summary>
        /// Test End of line returns true once all tokens retrieved.
        /// </summary>
        [TestMethod]
        public void ProgramLineEndOfLineIsTrueOnceTokensRead()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            programLine.NextToken();
            programLine.NextToken();
            Assert.IsTrue(programLine.EndOfLine);
        }

        /// <summary>
        /// Test End of line returns true if end of line token pushed back.
        /// </summary>
        [TestMethod]
        public void ProgramLineEndOfLineIsTrueIfEolIsPushedBack()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            programLine.NextToken();
            programLine.NextToken();
            var eolToken = programLine.NextToken();
            programLine.PushToken(eolToken);
            Assert.IsTrue(programLine.EndOfLine);
            var nextToken = programLine.NextToken();
            Assert.AreEqual(TokenType.EndOfLine, nextToken.Seperator);
        }

        /// <summary>
        /// Test End of line returns false if end of line token pushed back then previous tokens.
        /// </summary>
        [TestMethod]
        public void ProgramLineEndOfLineIsFalseIfEolPlusTokenIsPushedBack()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            var token1 = programLine.NextToken();
            var token2 = programLine.NextToken();
            var eolToken = programLine.NextToken();
            programLine.PushToken(eolToken);
            Assert.IsTrue(programLine.EndOfLine);
            programLine.PushToken(token2);
            programLine.PushToken(token1);
            Assert.IsFalse(programLine.EndOfLine);
            var nextToken = programLine.NextToken();
            Assert.AreEqual(token1, nextToken);
            nextToken = programLine.NextToken();
            Assert.AreEqual(token2, nextToken);
        }

        /// <summary>
        /// Test End of line returns false if not end of line token pushed back.
        /// </summary>
        [TestMethod]
        public void ProgramLineEndOfLineIsFalseIfLastTokenPushedBack()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            programLine.NextToken();
            var lastToken = programLine.NextToken();

            Assert.IsTrue(programLine.EndOfLine);
            programLine.PushToken(lastToken);
            Assert.IsFalse(programLine.EndOfLine);
        }

        /// <summary>
        /// Program line overrides ToString with a linenumber.
        /// </summary>
        [TestMethod]
        public void ProgramLineToStringWithLineNumber()
        {
            var tokens = new List<IToken>
            {
                new Token("PRINT", TokenClass.Statement),
                new Token("HELLO", TokenClass.String),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("W", TokenClass.Variable),
                new Token("$", TokenClass.Seperator, TokenType.Dollar),
            };

            ProgramLine programLine = new ProgramLine(30, tokens);
            var line = programLine.ToString();
            Assert.AreEqual("30  PRINT \"HELLO\",W$", line);
        }

        /// <summary>
        /// Program line overrides ToString with out a linenumber.
        /// </summary>
        [TestMethod]
        public void ProgramLineToStringWithOutLineNumber()
        {
            var tokens = new List<IToken>
            {
                new Token("PRINT", TokenClass.Statement),
                new Token("HELLO", TokenClass.String),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("W", TokenClass.Variable),
                new Token("$", TokenClass.Seperator, TokenType.Dollar),
            };

            ProgramLine programLine = new ProgramLine(null, tokens);
            var line = programLine.ToString();
            Assert.AreEqual("  PRINT \"HELLO\",W$", line);
        }

        /// <summary>
        /// Test last token if pushed back is returned.
        /// </summary>
        [TestMethod]
        public void IfLastTokenPushedBackThenNextTokenReturnsIt()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            programLine.NextToken();
            var lastToken = programLine.NextToken();

            Assert.IsTrue(programLine.EndOfLine);
            programLine.PushToken(lastToken);
            Assert.IsFalse(programLine.EndOfLine);

            var nextToken = programLine.NextToken();
            Assert.AreEqual("THREE", nextToken.Text);
        }

        /// <summary>
        /// Test if wrong token if pushed back exception is thrown.
        /// </summary>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void WrongTokenPushedBackThrowsException(bool throwsException)
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            var secondToken = programLine.NextToken();
            var thirdToken = programLine.NextToken();

            var token = throwsException ? secondToken : thirdToken;
            Test.Throws<InvalidOperationException>(() => programLine.PushToken(token), throwsException);
        }

        /// <summary>
        /// Test if token if pushed back at start of line exception is thrown.
        /// </summary>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void TokenPushedBackWhenAtStartOfLineThrowsException(bool throwsException)
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            var token = throwsException ? new Token(")", TokenClass.Seperator, TokenType.CloseBracket) : programLine.NextToken();
            Test.Throws<InvalidOperationException>(() => programLine.PushToken(token), throwsException);
        }

        /// <summary>
        /// Evaluate get line number
        /// </summary>
        [TestMethod]
        public void EvaluatorGetLine()
        {
            var tokens = new List<IToken>
            {
                new Token("2000")
            };
            ProgramLine programLine = new ProgramLine(30, tokens);
            var result = programLine.GetLineNumber();
            Assert.AreEqual(2000, result);
        }

        /// <summary>
        /// Evaluate get line number returns null if token is not number.
        /// </summary>
        [TestMethod]
        public void EvaluatorGetLineReturnsNullAndTokenIsNotEaten()
        {
            var tokens = new List<IToken>
            {
                new Token("DATA", TokenClass.Statement, TokenType.Data)
            };
            ProgramLine programLine = new ProgramLine(30, tokens);
            var result = programLine.GetLineNumber();
            Assert.AreEqual(null, result);
            Assert.AreEqual("DATA", programLine.NextToken().Text);
        }

        /// <summary>
        /// Evaluate get line number returns null if unparsable.
        /// </summary>
        [TestMethod]
        public void EvaluatorGetLineThrowsExceptionOnBadLineNumber()
        {
            var tokens = new List<IToken>
            {
                new Token("20X00")
            };
            ProgramLine programLine = new ProgramLine(30, tokens);
            var result = programLine.GetLineNumber();
            Assert.IsNull(result);
            Assert.AreEqual("20X00", programLine.NextToken().Text);
        }
    }
}
