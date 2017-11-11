// <copyright file="ReadTests.cs" company="Peter Ibbotson">
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
    /// Test class for Read.
    /// </summary>
    [TestClass]
    public class ReadTests
    {
        private Read _sut;
        private RunEnvironment _runEnvironment;
        private IVariableRepository _variableRepository;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IDataStatementReader> _mockDataStatementReader;
        private Mock<IReadInputParser> _mockReadInputParser;

        /// <summary>
        /// Test read.
        /// </summary>
        [TestMethod]
        public void ReadTest()
        {
            SetupSut();

            var variableA = _variableRepository.GetOrCreateVariable("A", new short[] { });
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken>());
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetLeftValue())
               .Returns(variableA);
            _sut.Execute();
            _mockReadInputParser.Verify(mrip => mrip.ReadVariables(new List<VariableReference> { variableA }), Times.Once);
        }

        /// <summary>
        /// Test read multiple variables.
        /// </summary>
        [TestMethod]
        public void ReadTestMultipleVariables()
        {
            SetupSut();

            var variableA = _variableRepository.GetOrCreateVariable("A", new short[] { });
            var variableB = _variableRepository.GetOrCreateVariable("B", new short[] { });
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { new Token(",", TokenType.ClassSeperator | TokenType.Comma) });
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetLeftValue())
               .Returns(variableA)
               .Returns(variableB);
            _sut.Execute();
            _mockReadInputParser.Verify(mrip => mrip.ReadVariables(new List<VariableReference> { variableA, variableB }), Times.Once);
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _mockDataStatementReader = new Mock<IDataStatementReader>();
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockReadInputParser = new Mock<IReadInputParser>();
            _mockDataStatementReader.Setup(mdsr => mdsr.ReadInputParser).Returns(_mockReadInputParser.Object);
            _sut = new Read(_runEnvironment, _mockExpressionEvaluator.Object, _mockDataStatementReader.Object);
        }
    }
}
