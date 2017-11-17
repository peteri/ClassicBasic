// <copyright file="EditTests.cs" company="Peter Ibbotson">
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
    /// Tests EDIT
    /// </summary>
    [TestClass]
    public class EditTests
    {
        private IProgramRepository _programRepository;
        private IRunEnvironment _runEnvironment;
        private MockTeletype _teletype;
        private Edit _sut;

        /// <summary>
        /// Test the edit command.
        /// </summary>
        [TestMethod]
        public void EditTest()
        {
            var tokens = new List<IToken>
            {
                new Token("20")
            };

            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            _sut.Execute();
            Assert.AreEqual("20 TWO", _teletype.EditText);
        }

        /// <summary>
        /// Test the edit command without a line number.
        /// </summary>
        [TestMethod]
        public void EditTestThrowsSyntaxWhenNoLineNumber()
        {
            var tokens = new List<IToken>
            {
                new Token("A")
            };
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test the edit command when in immediate mode.
        /// </summary>
        [TestMethod]
        public void EditTestThrowsCantEditWhenTeletypeCantEdit()
        {
            var tokens = new List<IToken>
            {
                new Token("20")
            };
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);
            bool exceptionThrown = false;
            try
            {
                _teletype.CanEdit = false;
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.UnableToEditException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test the edit command when in immediate mode.
        /// </summary>
        [TestMethod]
        public void EditTestThrowsSyntaxWhenNotInImmediateMode()
        {
            var tokens = new List<IToken>
            {
                new Token("20")
            };
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(20, tokens);
            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalDeferredException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _programRepository = new ProgramRepository();
            _runEnvironment = new RunEnvironment();
            _teletype = new MockTeletype
            {
                CanEdit = true
            };
            _programRepository.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("ONE") }));
            _programRepository.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("TWO") }));
            _programRepository.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("THREE") }));
            _programRepository.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("FOUR") }));
            _sut = new Edit(_runEnvironment, _programRepository, _teletype);
        }
    }
}
