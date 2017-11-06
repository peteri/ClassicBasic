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
            var variableRepository = new Mock<IVariableRepository>();
            var sut = new Clear(variableRepository.Object);
            sut.Execute();
            variableRepository.Verify(mvr => mvr.Clear(), Times.Once);
        }
    }
}
