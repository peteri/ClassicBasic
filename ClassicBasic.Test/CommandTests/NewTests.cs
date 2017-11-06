// <copyright file="NewTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the NEW statement.
    /// </summary>
    [TestClass]
    public class NewTests
    {
        /// <summary>
        /// Test new clears everything.
        /// </summary>
        [TestMethod]
        public void NewClearsEverything()
        {
            var runEnvironment = new RunEnvironment();
            runEnvironment.ContinueLineNumber = 1;
            runEnvironment.ProgramStack.Push(new StackEntry());
            runEnvironment.DefinedFunctions.Add("X", new UserDefinedFunction());
            var mockProgramRepository = new Mock<IProgramRepository>();
            var mockVariableRepository = new Mock<IVariableRepository>();
            var sut = new New(runEnvironment, mockProgramRepository.Object, mockVariableRepository.Object);
            sut.Execute();
            mockVariableRepository.Verify(mvr => mvr.Clear(), Times.Once);
            mockProgramRepository.Verify(mpr => mpr.Clear(), Times.Once);
            Assert.AreEqual(0, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(0, runEnvironment.DefinedFunctions.Count);
            Assert.IsFalse(runEnvironment.ContinueLineNumber.HasValue);
        }
    }
}
