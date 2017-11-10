// <copyright file="DataStatementReaderTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the data statement reader.
    /// </summary>
    [TestClass]
    public class DataStatementReaderTests
    {
        private DataStatementReader _sut;
        private Mock<IProgramRepository> _mockProgramRepository;

        /// <summary>
        /// Data statement reader does something.
        /// </summary>
        [TestMethod]
        public void DataStatementReaderDoesSomething()
        {
            SetupSut();
            Assert.Inconclusive();
        }

        private void SetupSut()
        {
            _mockProgramRepository = new Mock<IProgramRepository>();
            _sut = new DataStatementReader(_mockProgramRepository.Object);
        }
    }
}
