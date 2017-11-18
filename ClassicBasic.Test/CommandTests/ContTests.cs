// <copyright file="ContTests.cs" company="Peter Ibbotson">
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
    /// Tests for CONT command.
    /// </summary>
    [TestClass]
    public class ContTests
    {
        /// <summary>
        /// Tests that cont retrieves line and sets current line and token.
        /// </summary>
        [TestMethod]
        public void ContResetsTheCurrentLineAndToken()
        {
            var runEnvironment = new RunEnvironment();
            var mockProgramRepository = new Mock<IProgramRepository>();
            var continueLine = new ProgramLine(1000, new List<IToken> { });
            runEnvironment.CurrentLine = new ProgramLine(null, new List<IToken> { });
            runEnvironment.ContinueLineNumber = 1000;
            runEnvironment.ContinueToken = 5;
            mockProgramRepository.Setup(mpr => mpr.GetLine(1000)).Returns(continueLine);
            var sut = new Cont(runEnvironment, mockProgramRepository.Object);
            sut.Execute();
            Assert.AreEqual(continueLine, runEnvironment.CurrentLine);
            Assert.AreEqual(1000, runEnvironment.CurrentLine.LineNumber);
            Assert.AreEqual(5, runEnvironment.CurrentLine.CurrentToken);
        }

        /// <summary>
        /// Tests that CONT throws an exception if cont line is null.
        /// </summary>
        [TestMethod]
        public void ContThrowsExceptionIfNoContinueSet()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Cont(runEnvironment, Mock.Of<IProgramRepository>());
            Test.Throws<ClassicBasic.Interpreter.Exceptions.CantContinueException>(sut.Execute);
        }
    }
}
