// <copyright file="OnErrTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class for ON ERR.
    /// </summary>
    [TestClass]
    public class OnErrTests
    {
        private IRunEnvironment _runEnvironment;
        private OnErr _sut;

        /// <summary>
        /// ON ERR sets the error line handler.
        /// </summary>
        [TestMethod]
        public void OnErrSetsErrorLine()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("GOTO", TokenClass.Statement, TokenType.Goto),
                new Token("1000")
            };
            _runEnvironment.CurrentLine = new ProgramLine(20, tokens);
            _sut.Execute();
            Assert.AreEqual(1000, _runEnvironment.OnErrorGotoLineNumber);
        }

        /// <summary>
        /// ON ERR GOTO 0 clears the error line handler.
        /// </summary>
        [TestMethod]
        public void OnErrGotoZeroClearsErrorLine()
        {
            SetupSut();
            _runEnvironment.OnErrorGotoLineNumber = 100;
            var tokens = new List<IToken>
            {
                new Token("GOTO", TokenClass.Statement, TokenType.Goto),
                new Token("0")
            };
            _runEnvironment.CurrentLine = new ProgramLine(20, tokens);
            _sut.Execute();
            Assert.AreEqual(null, _runEnvironment.OnErrorGotoLineNumber);
        }

        /// <summary>
        /// ON ERR throws syntax error on missing goto.
        /// </summary>
        [TestMethod]
        public void OnErrThrowsSyntaxErrorOnMissingGoto()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("1000")
            };
            _runEnvironment.CurrentLine = new ProgramLine(20, tokens);
            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// ON ERR throws syntax error on missing goto.
        /// </summary>
        [TestMethod]
        public void OnErrThrowsSyntaxErrorOnMissingLineNumber()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("GOTO", TokenClass.Statement, TokenType.Goto),
                new Token("A")
            };
            _runEnvironment.CurrentLine = new ProgramLine(20, tokens);
            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _sut = new OnErr(_runEnvironment);
        }
    }
}
