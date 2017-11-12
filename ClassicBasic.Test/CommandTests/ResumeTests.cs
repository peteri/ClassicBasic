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
            _sut.Execute();
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// Test resume sets progrma line back.
        /// </summary>
        [TestMethod]
        public void ResumeSetsCurrentProgramLineAndToken()
        {
            SetupSut();
            _sut.Execute();
            Assert.AreEqual(20, _runEnvironment.CurrentLine.LineNumber);
            Assert.AreEqual(1, _runEnvironment.CurrentLine.CurrentToken);
        }

        /// <summary>
        /// Test resume with no error line number, clears error handler and throws exception..
        /// </summary>
        [TestMethod]
        public void ResumeResets()
        {
            bool exceptionThrown = false;
            SetupSut();
            _runEnvironment.LastErrorLine = null;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.UndefinedStatementException)
            {
                exceptionThrown = true;
            }

            Assert.IsNull( _runEnvironment.OnErrorGotoLineNumber);
            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _programRepository = new Mock<IProgramRepository>();
            _runEnvironment = new RunEnvironment();

            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.LastErrorStackCount = _runEnvironment.ProgramStack.Count;
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.LastErrorLine = 20;
            _runEnvironment.LastErrorNumber = 999;
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
