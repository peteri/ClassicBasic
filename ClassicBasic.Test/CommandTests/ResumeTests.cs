// <copyright file="ResumeTests.cs" company="Peter Ibbotson">
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
    /// Test class for RESUME.
    /// </summary>
    [TestClass]
    public class ResumeTests
    {
        private Mock<IProgramRepository> _programRepository;
        private IRunEnvironment _runEnvironment;
        private Resume _sut;

        /// <summary>
        /// Test resume pops any new entries off the stack.
        /// </summary>
        [TestMethod]
        public void ResumePopsAnyProgramStackEntries()
        {
            SetupSut();
        }

        private void SetupSut()
        {
            _programRepository = new Mock<IProgramRepository>();
            _runEnvironment = new RunEnvironment();

            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.LastErrorStackCount = _runEnvironment.ProgramStack.Count;
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.LastErrorNumber = 20;
            _runEnvironment.LastErrorToken = 1;

            var tokens = new List<IToken>
            {
                new Token("1"),
                new Token("2"),
                new Token("3")
            };
            _programRepository.Setup(mpr => mpr.GetLine(20))
                .Returns(new ProgramLine(20, tokens));
            _sut = new Resume(_runEnvironment, _programRepository.Object);
        }
    }
}
