// <copyright file="PrintTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System;
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the PRINT command.
    /// </summary>
    [TestClass]
    public class PrintTests
    {
        private Print _sut;
        private IRunEnvironment _runEnvironment;
        private IExpressionEvaluator _expressionEvaluator;
        private TeletypeWithPosition _teletypeWithPosition;
        private MockTeletype _teletype;
        private Mock<IVariableRepository> _mockVariableRepository;

        /// <summary>
        /// Tests prints HELLO and terminates on colon.
        /// </summary>
        [TestMethod]
        public void PrintPrintsString()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();

            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Tests prints HELLO and terminates on colon.
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringAndStopsLoopingOnColon()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();
            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Tests prints HELLO and terminates on else.
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringAndStopsLoopingOnElse()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token("ELSE", TokenType.ClassStatement | TokenType.Else)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();

            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Test prints HELLO with a semicolon doesn't call comma or Newline
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringFollowedBySemicolonWithoutNewline()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token(";", TokenType.ClassSeperator | TokenType.Semicolon),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();
            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Test prints HELLO with a comma.
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringFollowedByCommaWithoutNewline()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token(",", TokenType.ClassSeperator | TokenType.Comma),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();
            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(new string(' ', 14 - 5), _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Test prints HELLO with a comma and expression evaluates expression.
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringFollowedExpressionWithNewline()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token(",", TokenType.ClassSeperator | TokenType.Comma),
                new Token("3"),
                new Token("+", TokenType.ClassSeperator | TokenType.Plus),
                new Token("3"),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();

            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(new string(' ', 14 - 5), _teletype.Output.Dequeue());
            Assert.AreEqual("6", _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Test prints HELLO with a comma and expression evaluates expression.
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringFollowedExpressionWithoutNewline()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token(";", TokenType.ClassSeperator | TokenType.Semicolon),
                new Token("4"),
                new Token("*", TokenType.ClassSeperator | TokenType.Multiply),
                new Token("4"),
                new Token(";", TokenType.ClassSeperator | TokenType.Semicolon),
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();

            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual("16", _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Test prints HELLO followed by SPC(3+3) gives expected result.
        /// </summary>
        [TestMethod]
        public void PrintPrintsUsingSpaceCallsTeletypeCorrectly()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token("SPC(", TokenType.ClassStatement | TokenType.Space),
                new Token("3"),
                new Token("+", TokenType.ClassSeperator | TokenType.Plus),
                new Token("3"),
                new Token(")", TokenType.ClassSeperator | TokenType.CloseBracket),
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();

            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(new string(' ', 6), _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual(0, _teletype.Output.Count);
        }

        /// <summary>
        /// Test prints HELLO with tab does expected result.
        /// </summary>
        [TestMethod]
        public void PrintPrintsUsingTabAdvancesTeletypeCorrectly()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token("TAB(", TokenType.ClassStatement | TokenType.Tab),
                new Token("9"),
                new Token(")", TokenType.ClassSeperator | TokenType.CloseBracket),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();

            Assert.AreEqual("HELLO", _teletype.Output.Dequeue());
            Assert.AreEqual(new string(' ', 3), _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
        }

        /// <summary>
        /// Test prints which uses tab without a closing base throws SyntaxError
        /// </summary>
        [TestMethod]
        public void PrintPrintsUsingTabThrowsIfNoCloseBracket()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token("TAB(", TokenType.ClassStatement | TokenType.Tab),
                new Token("9"),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
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
            _mockVariableRepository = new Mock<IVariableRepository>();
            _teletype = new MockTeletype();
            _teletypeWithPosition = new TeletypeWithPosition(_teletype);
            _expressionEvaluator = new ExpressionEvaluator(_mockVariableRepository.Object, _runEnvironment);
            _sut = new Print(_runEnvironment, _expressionEvaluator, _teletypeWithPosition);
        }
    }
}
