// <copyright file="GotoTests.cs" company="Peter Ibbotson">
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
    /// Tests GOTO
    /// </summary>
    [TestClass]
    public class GotoTests
    {
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IProgramRepository> _mockProgramRepository;
        private IRunEnvironment _runEnvironment;
        private ProgramLine _gotoProgramLine;
        private ProgramLine _targetProgramLine;
        private Goto _sut;

        /// <summary>
        /// Tests GOTO sets current line to target.
        /// </summary>
        [TestMethod]
        public void GotoSetsCurrentLineToTarget()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { new Token("100") });
            _sut.Execute();
            _mockProgramRepository.Verify(mpr => mpr.GetLine(100), Times.Once);
            Assert.AreEqual(100, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(_targetProgramLine, _runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Tests GOTO Throws exception if line number does not exist.
        /// </summary>
        [TestMethod]
        public void GotoThrowsExceptionIfLineNumberDoesExist()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { new Token("110") });
            var exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.UndefinedStatementException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests GOTO Throws exception if no line number.
        /// </summary>
        [TestMethod]
        public void GotoThrowsExceptionIfNoLineNumber()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { new Token("A") });
            var exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.UndefinedStatementException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _runEnvironment = new RunEnvironment();
            _gotoProgramLine = new ProgramLine(10, new List<IToken> { });
            _targetProgramLine = new ProgramLine(100, new List<IToken> { });

            _sut = new Goto(_runEnvironment, _mockExpressionEvaluator.Object, _mockProgramRepository.Object);
            _mockProgramRepository.Setup(mpr => mpr.GetLine(100)).Returns(_targetProgramLine);
            _mockProgramRepository.Setup(mpr => mpr.GetLine(110))
                .Throws(new ClassicBasic.Interpreter.Exceptions.UndefinedStatementException());
            _runEnvironment.CurrentLine = _gotoProgramLine;
            _gotoProgramLine.CurrentToken = 2;
        }
    }
}
