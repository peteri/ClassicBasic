// <copyright file="ReadTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
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
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IDataStatementReader> _mockDataStatementReader;

        /// <summary>
        /// Test read.
        /// </summary>
        [TestMethod]
        public void ReadTest()
        {
            SetupSut();
            Assert.Inconclusive();
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _mockDataStatementReader = new Mock<IDataStatementReader>();
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _sut = new Read(_runEnvironment, _mockExpressionEvaluator.Object, _mockDataStatementReader.Object);
        }
    }
}
