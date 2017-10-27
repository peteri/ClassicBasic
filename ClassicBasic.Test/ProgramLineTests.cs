// <copyright file="ProgramLineTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using System;
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Program line class.
    /// </summary>
    [TestClass]
    public class ProgramLineTests
    {
        private List<IToken> _tokens = new List<IToken>
        {
            new Token("ONE", TokenType.ClassVariable),
            new Token("TWO", TokenType.ClassVariable),
            new Token("THREE", TokenType.ClassVariable)
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
        [TestMethod]
        public void WrongTokenPushedBackThrowsException()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);
            programLine.NextToken();
            var secondToken = programLine.NextToken();
            programLine.NextToken();

            bool exceptionThrown = false;
            try
            {
                programLine.PushToken(secondToken);
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test if token if pushed back at start of line exception is thrown.
        /// </summary>
        [TestMethod]
        public void TokenPushedBackWhenAtStartOfLineThrowsException()
        {
            ProgramLine programLine = new ProgramLine(30, _tokens);

            bool exceptionThrown = false;
            try
            {
                programLine.PushToken(new Token("EH", TokenType.CloseBracket));
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
