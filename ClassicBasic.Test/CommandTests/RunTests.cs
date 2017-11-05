// <copyright file="RunTests.cs" company="Peter Ibbotson">
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
    /// Test class for RUN.
    /// </summary>
    [TestClass]
    public class RunTests
    {
        private Run _sut;
        private IRunEnvironment _runEnvironment;
        private ProgramLine _line10;
        private ProgramLine _line20;
        private Mock<ITokeniser> _mockTokeniser;
        private Mock<IProgramRepository> _mockProgramRepository;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IVariableRepository> _mockVariableRepository;

        /// <summary>
        /// Run clears Program stack and variables.
        /// </summary>
        [TestMethod]
        public void RunClearsVariablesAndStacks()
        {
            SetupSut();
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _sut.Execute(_mockTokeniser.Object);
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber);
            _mockVariableRepository.Verify(mvr => mvr.Clear(), Times.Once);
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// Run starts from a line number.
        /// </summary>
        [TestMethod]
        public void RunStartsFromALineNumber()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetLineNumber()).Returns(20);
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _sut.Execute(_mockTokeniser.Object);
            Assert.AreEqual(20, _runEnvironment.CurrentLine.LineNumber);
        }

        /// <summary>
        /// Run starts from a line number.
        /// </summary>
        [TestMethod]
        public void RunExecutesLoad()
        {
            SetupSut();
            var mockLoadToken = new Mock<IToken>();
            var mockLoadCmd = mockLoadToken.As<ITokeniserCommand>();
            _mockTokeniser.Setup(mt => mt.Tokenise("LOAD \"test.bas\""))
                .Returns(new ProgramLine(null, new List<IToken> { mockLoadToken.Object }));
            _runEnvironment.CurrentLine = new ProgramLine(null, new List<IToken> { new Token("test.bas", TokenType.ClassString) });
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _sut.Execute(_mockTokeniser.Object);
            mockLoadCmd.Verify(mlc => mlc.Execute(_mockTokeniser.Object), Times.Once);
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _runEnvironment.CurrentLine = new ProgramLine(null, new List<IToken> { });
            _line10 = new ProgramLine(10, new List<IToken> { });
            _line20 = new ProgramLine(20, new List<IToken> { });
            _mockTokeniser = new Mock<ITokeniser>();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _mockProgramRepository.Setup(mpr => mpr.GetFirstLine()).Returns(_line10);
            _mockProgramRepository.Setup(mpr => mpr.GetLine(20)).Returns(_line20);
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _sut = new Run(
                _runEnvironment,
                _mockProgramRepository.Object,
                _mockExpressionEvaluator.Object,
                _mockVariableRepository.Object);
        }
    }
}
