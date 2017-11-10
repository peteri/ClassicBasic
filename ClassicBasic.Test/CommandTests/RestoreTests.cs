// <copyright file="RestoreTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class for restore.
    /// </summary>
    [TestClass]
    public class RestoreTests
    {
        private Restore _sut;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IDataStatementReader> _mockDataStatementReader;

        /// <summary>
        /// Test restore without line number calls datastatement reader restore.
        /// </summary>
        [TestMethod]
        public void RestoreWithoutLineNumber()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetLineNumber()).Returns((int?)null);
            _sut.Execute();
            _mockDataStatementReader.Verify(mdsr => mdsr.RestoreToLineNumber(null), Times.Once);
        }

        /// <summary>
        /// Test restore with line number calls datastatement reader restore.
        /// </summary>
        [TestMethod]
        public void RestoreWithLineNumber()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetLineNumber()).Returns(1000);
            _sut.Execute();
            _mockDataStatementReader.Verify(mdsr => mdsr.RestoreToLineNumber(1000), Times.Once);
        }

        private void SetupSut()
        {
            _mockDataStatementReader = new Mock<IDataStatementReader>();
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _sut = new Restore(_mockExpressionEvaluator.Object, _mockDataStatementReader.Object);
        }
    }
}
