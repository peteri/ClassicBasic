// <copyright file="EndTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test the end exception.
    /// </summary>
    [TestClass]
    public class EndTests
    {
        /// <summary>
        /// Check end throws the correct exception and stores the
        /// correct position (after the end) in the continue token.
        /// </summary>
        [TestMethod]
        public void EndTest()
        {
            var mockRunEnvironment = new Mock<IRunEnvironment>();
            var programLine = new ProgramLine(30, new List<IToken> { new Token("Name"), new Token("1") });
            programLine.NextToken();
            mockRunEnvironment.Setup(mre => mre.CurrentLine).Returns(programLine);
            var sut = new End(mockRunEnvironment.Object);
            Test.Throws<EndException>(sut.Execute);
            mockRunEnvironment.VerifySet(mre => mre.ContinueToken = 1);
        }
    }
}
