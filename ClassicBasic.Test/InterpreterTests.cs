// <copyright file="InterpreterTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the interpreter.
    /// </summary>
    [TestClass]
    public class InterpreterTests
    {
        private MockTeletype _mockTeletype;
        private Mock<ITokeniser> _mockTokeniser;
        private Mock<IRunEnvironment> _mockRunEnvironment;
        private Mock<IProgramRepository> _mockProgramRepository;
        private Mock<IExecutor> _mockExecute;

        /// <summary>
        /// To Do.
        /// </summary>
        [TestMethod]
        public void ToDo()
        {
            _mockTeletype = new MockTeletype();
            _mockTokeniser = new Mock<ITokeniser>();
            _mockRunEnvironment = new Mock<IRunEnvironment>();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _mockExecute = new Mock<IExecutor>();
            IInterpreter sut = new Interpreter(
                _mockTeletype,
                _mockTokeniser.Object,
                _mockRunEnvironment.Object,
                _mockProgramRepository.Object,
                _mockExecute.Object);
            throw new AssertInconclusiveException();
        }
    }
}
