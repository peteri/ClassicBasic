// <copyright file="DimTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
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
        private Dim _sut;

        /// <summary>
        /// Tests dim does the right thing.
        /// </summary>
        [TestMethod]
        public void TestDim()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _mockExpressionEvaluator.Setup(mee => mee.GetIndexes()).Returns(new short[] { 3, 2 });
            _mockExpressionEvaluator.Setup(mee => mee.GetVariableName()).Returns("A");
            _sut = new Dim(_mockExpressionEvaluator.Object, _mockVariableRepository.Object);
            _sut.Execute();
            _mockVariableRepository.Verify(mvr => mvr.DimensionArray("A", new short[] { 3, 2 }), Times.Once);
        }

        /// <summary>
        /// Tests dim skips calling DimensionArray.
        /// </summary>
        [TestMethod]
        public void TestDimWithEmptyIndexesSkipsCreate()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockVariableRepository = new Mock<IVariableRepository>();
            _mockExpressionEvaluator.Setup(mee => mee.GetIndexes()).Returns(new short[] { });
            _mockExpressionEvaluator.Setup(mee => mee.GetVariableName()).Returns("A");
            _sut = new Dim(_mockExpressionEvaluator.Object, _mockVariableRepository.Object);
            _sut.Execute();
            _mockVariableRepository.Verify(mvr => mvr.DimensionArray(It.IsAny<string>(), It.IsAny<short[]>()), Times.Never);
        }
    }
}
