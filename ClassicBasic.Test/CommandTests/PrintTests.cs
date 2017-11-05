// <copyright file="PrintTests.cs" company="Peter Ibbotson">
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
    /// Tests the PRINT command.
    /// </summary>
    [TestClass]
    public class PrintTests
    {
        private Print _sut;
        private IRunEnvironment _runEnvironment;
        private IExpressionEvaluator _expressionEvaluator;
        private Mock<ITeletypeWithPosition> _teletype;
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
        }

        /// <summary>
        /// Test prints HELLO with a semicolon doesn't call comma or Newline
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
        }

        /// <summary>
        /// Test prints HELLO with a semicolon doesn't call comma or Newline
        /// </summary>
        [TestMethod]
        public void PrintPrintsStringFollowedExpressionWithoutNewline()
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
        }

        /// <summary>
        /// Test prints HELLO with a semicolon doesn't call comma or Newline
        /// </summary>
        [TestMethod]
        public void PrintPrintsUsingSpaceCallsTeletypeFunctionNewline()
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
        }

        /// <summary>
        /// Test prints HELLO with a semicolon doesn't call comma or Newline
        /// </summary>
        [TestMethod]
        public void PrintPrintsUsingTaballsTeletypeFunctionNewline()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token("TAB(", TokenType.ClassStatement | TokenType.Tab),
                new Token("3"),
                new Token(")", TokenType.ClassSeperator | TokenType.CloseBracket),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();
        }

        /// <summary>
        /// Test prints HELLO with a semicolon doesn't call comma or Newline
        /// </summary>
        [TestMethod]
        public void PrintPrintsUsingTabThrowsIfNoCloseBracket()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("HELLO", TokenType.ClassString),
                new Token("TAB(", TokenType.ClassStatement | TokenType.Tab),
                new Token("3"),
                new Token(":", TokenType.ClassSeperator | TokenType.Colon)
            };

            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _teletype = new Mock<ITeletypeWithPosition>();
            _expressionEvaluator = new ExpressionEvaluator(_mockVariableRepository.Object, _runEnvironment);
            _sut = new Print(_runEnvironment, _expressionEvaluator, _teletype.Object);
        }
    }
}
