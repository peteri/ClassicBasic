// <copyright file="DataTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the DATA statement.
    /// </summary>
    [TestClass]
    public class DataTests
    {
        /// <summary>
        /// Data statement moves to end of line.
        /// </summary>
        [TestMethod]
        public void DataSkipsToEndOfLine()
        {
            var runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(10, new List<IToken> { new Token("1,abc def, ", TokenClass.Data) })
            };
            var sut = new Data(runEnvironment);
            sut.Execute();
            Assert.IsTrue(runEnvironment.CurrentLine.EndOfLine);
        }

        /// <summary>
        /// Data statement moves to next statement.
        /// </summary>
        [TestMethod]
        public void DataSkipsToNextStatement()
        {
            var runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(
                10,
                new List<IToken> { new Token("1", TokenClass.Data), new Token(":", TokenClass.Seperator, TokenType.Colon) })
            };
            var sut = new Data(runEnvironment);
            sut.Execute();
            Assert.AreEqual(TokenType.Colon, runEnvironment.CurrentLine.NextToken().Seperator);
        }

        /// <summary>
        /// Data statement Throws Exception in immediate mode.
        /// </summary>
        [TestMethod]
        public void DataSkipsThrowsExceptionInImmediateMode()
        {
            var runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(
                null,
                new List<IToken> { new Token("1"), new Token(":", TokenClass.Seperator, TokenType.Colon) })
            };
            var sut = new Data(runEnvironment);
            var exceptionThrown = false;
            try
            {
                sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalDirectException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Data statement Throws Syntax error if not followed by ClassData.
        /// </summary>
        [TestMethod]
        public void DataSkipsThrowsSyntaxExceptionIfNotFollowedByClassData()
        {
            var runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(
                10,
                new List<IToken> { new Token("1"), new Token(":", TokenClass.Seperator, TokenType.Colon) })
            };
            var sut = new Data(runEnvironment);
            var exceptionThrown = false;
            try
            {
                sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
