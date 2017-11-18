// <copyright file="OnTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the ON statement.
    /// </summary>
    [TestClass]
    public class OnTests
    {
        private On _sut;
        private IRunEnvironment _runEnvironment;
        private Mock<IProgramRepository> _mockProgramRepository;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;

        /// <summary>
        /// Test On calls goto correctly.
        /// </summary>
        /// <param name="value">Value to jump to.</param>
        /// <param name="expectedProgramLine">Expected final program line.</param>
        /// <param name="throwsInvalidQuantity">Flag to say what the result is.</param>
        [DataTestMethod]
        [DataRow(-1.0, 10, true)]
        [DataRow(0.0, 10, false)]
        [DataRow(1.0, 1000, false)]
        [DataRow(2.0, 2000, false)]
        [DataRow(3.0, 3000, false)]
        [DataRow(4.0, 10, false)]
        [DataRow(256.0, 10, true)]
        public void OnCallsGoto(double value, int expectedProgramLine, bool throwsInvalidQuantity)
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(value));
            var tokens = new List<IToken>
            {
                new Token("GOTO", TokenClass.Statement, TokenType.Goto),
                new Token("1000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("2000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("3000")
            };

            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            bool invalidQuantityThrown = false;
            try
            {
                _sut.Execute();
                Assert.AreEqual(expectedProgramLine, _runEnvironment.CurrentLine.LineNumber);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                invalidQuantityThrown = true;
            }

            Assert.AreEqual(throwsInvalidQuantity, invalidQuantityThrown);
        }

        /// <summary>
        /// Test On calls goto correctly.
        /// </summary>
        /// <param name="value">Value to jump to.</param>
        /// <param name="expectedProgramLine">Expected final program line.</param>
        /// <param name="throwsInvalidQuantity">Flag to say what the result is.</param>
        [DataTestMethod]
        [DataRow(-1.0, 10, true)]
        [DataRow(0.0, 10, false)]
        [DataRow(1.0, 1000, false)]
        [DataRow(2.0, 2000, false)]
        [DataRow(3.0, 3000, false)]
        [DataRow(4.0, 10, false)]
        [DataRow(256.0, 10, true)]
        public void OnCallsGosub(double value, int expectedProgramLine, bool throwsInvalidQuantity)
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(value));
            var tokens = new List<IToken>
            {
                new Token("GOSUB", TokenClass.Statement, TokenType.Gosub),
                new Token("1000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("2000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("3000"),
                new Token(":", TokenClass.Seperator, TokenType.Colon),
            };

            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            bool invalidQuantityThrown = false;
            try
            {
                _sut.Execute();
                Assert.AreEqual(expectedProgramLine, _runEnvironment.CurrentLine.LineNumber);
                if (expectedProgramLine == 10)
                {
                    Assert.AreEqual(TokenType.Colon, _runEnvironment.CurrentLine.NextToken().Seperator);
                }
                else
                {
                    var stackEntry = _runEnvironment.ProgramStack.Pop();
                    Assert.AreEqual(TokenType.Colon, stackEntry.Line.NextToken().Seperator);
                    Assert.AreEqual(10, stackEntry.Line.LineNumber);
                    Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
                }
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                invalidQuantityThrown = true;
            }

            Assert.AreEqual(throwsInvalidQuantity, invalidQuantityThrown);
        }

        /// <summary>
        /// Test On requires GOTO or GOSUB.
        /// </summary>
        [TestMethod]
        public void OnRequiresGotoOrGosub()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(2.0));
            var tokens = new List<IToken>
            {
                new Token("TO", TokenClass.Statement, TokenType.To),
                new Token("1000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("2000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("3000"),
                new Token(":", TokenClass.Seperator, TokenType.Colon),
            };

            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            Test.Throws<SyntaxErrorException>(_sut.Execute);
        }

        /// <summary>
        /// Test On stops parsing on missing comma.
        /// </summary>
        [TestMethod]
        public void OnStopsParsingOnMissingComma()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(2.0));
            var tokens = new List<IToken>
            {
                new Token("GOSUB", TokenClass.Statement, TokenType.Gosub),
                new Token("1000"),
                new Token("2000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("3000"),
                new Token(":", TokenClass.Seperator, TokenType.Colon),
            };

            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);
            _sut.Execute();
            Assert.AreEqual("2000", _runEnvironment.CurrentLine.NextToken().Text);
        }

        /// <summary>
        /// Test On throws SyntaxErrorOnMissingLineNumber.
        /// </summary>
        [TestMethod]
        public void OnThrowsSyntaxErrorOnMissingLineNumber()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(2.0));
            var tokens = new List<IToken>
            {
                new Token("GOSUB", TokenClass.Statement, TokenType.Gosub),
                new Token("1000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("3000"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token(":", TokenClass.Seperator, TokenType.Colon),
            };

            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);
            Test.Throws<SyntaxErrorException>(_sut.Execute);
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _mockProgramRepository.Setup(mpr => mpr.GetLine(1000)).Returns(new ProgramLine(1000, new List<IToken> { }));
            _mockProgramRepository.Setup(mpr => mpr.GetLine(2000)).Returns(new ProgramLine(2000, new List<IToken> { }));
            _mockProgramRepository.Setup(mpr => mpr.GetLine(3000)).Returns(new ProgramLine(3000, new List<IToken> { }));
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _sut = new On(_runEnvironment, _mockExpressionEvaluator.Object, _mockProgramRepository.Object);
        }
    }
}
