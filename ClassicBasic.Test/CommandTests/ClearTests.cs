// <copyright file="ClearTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the CLEAR statement.
    /// </summary>
    [TestClass]
    public class ClearTests
    {
        /// <summary>
        /// Tests clear clears the variable repository.
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            var runEnvironment = new RunEnvironment
            {
                ContinueLineNumber = 1
            };
            runEnvironment.ProgramStack.Push(new StackEntry());
            runEnvironment.DefinedFunctions.Add("X", new UserDefinedFunction());
            var mockVariableRepository = new Mock<IVariableRepository>();
            var mockDataStatementReader = new Mock<IDataStatementReader>();

            var sut = new Clear(
                    runEnvironment,
                    mockVariableRepository.Object,
                    mockDataStatementReader.Object);
            sut.Execute();
            mockVariableRepository.Verify(mvr => mvr.Clear(), Times.Once);
            mockDataStatementReader.Verify(mdsr => mdsr.RestoreToLineNumber(null), Times.Once);
            Assert.AreEqual(0, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(0, runEnvironment.DefinedFunctions.Count);
            Assert.IsFalse(runEnvironment.ContinueLineNumber.HasValue);
        }
    }
}
