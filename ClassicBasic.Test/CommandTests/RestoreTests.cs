// <copyright file="RestoreTests.cs" company="Peter Ibbotson">
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
    /// Test class for restore.
    /// </summary>
    [TestClass]
    public class RestoreTests
    {
        private Restore _sut;
        private IRunEnvironment _runEnvironment;
        private Mock<IDataStatementReader> _mockDataStatementReader;

        /// <summary>
        /// Test restore without line number calls datastatement reader restore.
        /// </summary>
        [TestMethod]
        public void RestoreWithoutLineNumber()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { new Token("A") });
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
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { new Token("1000") });
            _sut.Execute();
            _mockDataStatementReader.Verify(mdsr => mdsr.RestoreToLineNumber(1000), Times.Once);
        }

        private void SetupSut()
        {
            _mockDataStatementReader = new Mock<IDataStatementReader>();
            _runEnvironment = new RunEnvironment();
            _sut = new Restore(_runEnvironment, _mockDataStatementReader.Object);
        }
    }
}
