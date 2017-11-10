// <copyright file="DimTests.cs" company="Peter Ibbotson">
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
    /// Tests the DIM statement
    /// </summary>
    [TestClass]
    public class DimTests
    {
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IVariableRepository> _mockVariableRepository;
        private RunEnvironment _runEnvironment;
        private Dim _sut;

        /// <summary>
        /// Tests dim does the right thing.
        /// </summary>
        [TestMethod]
        public void TestDim()
        {
            _runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(0, new List<IToken> { })
            };
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _mockExpressionEvaluator.Setup(mee => mee.GetIndexes()).Returns(new short[] { 3, 2 });
            _mockExpressionEvaluator.Setup(mee => mee.GetVariableName()).Returns("A");
            _sut = new Dim(_runEnvironment, _mockExpressionEvaluator.Object, _mockVariableRepository.Object);
            _sut.Execute();
            _mockVariableRepository.Verify(mvr => mvr.DimensionArray("A", new short[] { 3, 2 }), Times.Once);
        }

        /// <summary>
        /// Tests dim defines multiple arrays.
        /// </summary>
        [TestMethod]
        public void TestDimMultipleArrays()
        {
            _runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(0, new List<IToken> { new Token(",", TokenType.ClassSeperator | TokenType.Comma) })
            };
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetIndexes())
                .Returns(new short[] { 3, 2 })
                .Returns(new short[] { 11, 12 });
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetVariableName())
                .Returns("A")
                .Returns("B$");
            _sut = new Dim(_runEnvironment, _mockExpressionEvaluator.Object, _mockVariableRepository.Object);
            _sut.Execute();
            _mockVariableRepository.Verify(mvr => mvr.DimensionArray("A", new short[] { 3, 2 }), Times.Once);
            _mockVariableRepository.Verify(mvr => mvr.DimensionArray("B$", new short[] { 11, 12 }), Times.Once);
        }

        /// <summary>
        /// Tests dim skips calling DimensionArray.
        /// </summary>
        [TestMethod]
        public void TestDimWithEmptyIndexesSkipsCreate()
        {
            _runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(0, new List<IToken> { })
            };
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _mockExpressionEvaluator.Setup(mee => mee.GetIndexes()).Returns(new short[] { });
            _mockExpressionEvaluator.Setup(mee => mee.GetVariableName()).Returns("A");
            _sut = new Dim(_runEnvironment, _mockExpressionEvaluator.Object, _mockVariableRepository.Object);
            _sut.Execute();
            _mockVariableRepository.Verify(mvr => mvr.DimensionArray(It.IsAny<string>(), It.IsAny<short[]>()), Times.Never);
        }
    }
}
