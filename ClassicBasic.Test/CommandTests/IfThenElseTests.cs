// <copyright file="IfThenElseTests.cs" company="Peter Ibbotson">
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
    /// Test class for IF THEN ELSE
    /// </summary>
    [TestClass]
    public class IfThenElseTests
    {
        private If _sut;
        private IRunEnvironment _runEnvironment;
        private Mock<IProgramRepository> _mockProgramRepository;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;

        /// <summary>
        /// Test that IF moves to after THEN when expression is non zero.
        /// </summary>
        [TestMethod]
        public void IfExpressionNonZeroDoubleThenTokenMovesToAfterThen()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0));

            _sut.Execute();

            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("TRUE", token.Text);
        }

        /// <summary>
        /// Test that ELSE if executed skips to end of line.
        /// </summary>
        [TestMethod]
        public void IfExpressionNonZeroDoubleWhenElseIsExecutedTokenSkipsToEndOfLine()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0));

            _sut.Execute();

            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("TRUE", token.Text);
            token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("STMT1", token.Text);
            ICommand sutElse = _runEnvironment.CurrentLine.NextToken() as ICommand;
            sutElse.Execute();

            Assert.IsTrue(_runEnvironment.CurrentLine.EndOfLine);
        }

        /// <summary>
        /// Test that IF moves to after THEN when expression is non empty string.
        /// </summary>
        [TestMethod]
        public void IfExpressionNonEmptyStringThenTokenMovesToAfterThen()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));

            _sut.Execute();

            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("TRUE", token.Text);
        }

        /// <summary>
        /// Test that IF moves to after ELSE when expression is zero.
        /// </summary>
        [TestMethod]
        public void IfExpressionZeroDoubleThenTokenMovesToAfterElse()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(0.0));

            _sut.Execute();

            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("FALSE", token.Text);
        }

        /// <summary>
        /// Test that IF moves to end of line if no ELSE when expression is zero.
        /// </summary>
        [TestMethod]
        public void IfExpressionZeroDoubleThenTokenMovesToEndOfLine()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(0.0));
            _runEnvironment.CurrentLine = new ProgramLine(
                10,
#pragma warning disable SA1118 // Parameter must not span multiple lines
                new List<IToken>
                {
                    new Token("THEN", TokenType.ClassStatement | TokenType.Then),
                    new Token("TRUE"),
                    new Token("STMT1")
                });
#pragma warning restore SA1118 // Parameter must not span multiple lines

            _sut.Execute();

            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.IsTrue(_runEnvironment.CurrentLine.EndOfLine);
        }

        /// <summary>
        /// Test that IF moves to after ELSE when expression is empty string.
        /// </summary>
        [TestMethod]
        public void IfExpressionEmptyStringThenTokenMovesToAfterElse()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(string.Empty));

            _sut.Execute();

            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("FALSE", token.Text);
        }

        /// <summary>
        /// Test that IF throws syntax error if token is not THEN or GOTO.
        /// </summary>
        [TestMethod]
        public void IfExpressionThrowsIfNextTokenIsThenOrGoto()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(string.Empty));
            _runEnvironment.CurrentLine = new ProgramLine(
                10,
                new List<IToken> { new Token("ELSE", TokenType.ClassStatement | TokenType.Else) });

            bool throwsException = false;

            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                throwsException = true;
            }

            Assert.IsTrue(throwsException);
        }

        /// <summary>
        /// Test that IF Calls program repostiory if followed by number.
        /// </summary>
        [TestMethod]
        public void IfExpressionNonZeroDoubleThenFollowedByNumberJumpsToLine()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0));
            _mockProgramRepository.Setup(mpr => mpr.GetLine(100))
                .Returns(new ProgramLine(100, new List<IToken> { }));
            var tokens = new List<IToken>
            {
                new Token("THEN", TokenType.ClassStatement | TokenType.Then),
                new Token("100")
            };

            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            _sut.Execute();

            Assert.AreEqual(100, _runEnvironment.CurrentLine.LineNumber);
        }

        /// <summary>
        /// Test that IF Calls program repostiory if followed by goto.
        /// </summary>
        [TestMethod]
        public void IfExpressionNonZeroDoubleGotoFollowedByNumberJumpsToLine()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0));
            _mockProgramRepository.Setup(mpr => mpr.GetLine(100))
                .Returns(new ProgramLine(100, new List<IToken> { }));
            var tokens = new List<IToken>
            {
                new Token("GOTO", TokenType.ClassStatement | TokenType.Goto),
                new Token("100")
            };
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            _sut.Execute();

            Assert.AreEqual(100, _runEnvironment.CurrentLine.LineNumber);
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _sut = new If(_runEnvironment, _mockExpressionEvaluator.Object, _mockProgramRepository.Object);

            _runEnvironment.CurrentLine = new ProgramLine(
                10,
#pragma warning disable SA1118 // Parameter must not span multiple lines
                new List<IToken>
                {
                    new Token("THEN", TokenType.ClassStatement | TokenType.Then),
                    new Token("TRUE"),
                    new Token("STMT1"),
                    new Else(_runEnvironment),
                    new Token("FALSE"),
                    new Token("STMT1"),
                });
#pragma warning restore SA1118 // Parameter must not span multiple lines
        }
    }
}
