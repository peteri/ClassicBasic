// <copyright file="InterpreterTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System;
    using System.Collections.Generic;
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
        private Mock<IExecutor> _mockExecutor;
        private IInterpreter _sut;

        /// <summary>
        /// We exit when the user types system....
        /// </summary>
        [TestMethod]
        public void InterpreterReturnsWhenExecutorReturnsTrue()
        {
            SetupSut();

            _mockTeletype.Input.Enqueue("SYSTEM");

            _sut.Execute();
        }

        /// <summary>
        /// Check interpreter adds program lines.
        /// </summary>
        [TestMethod]
        public void InterpreterAddsProgramLines()
        {
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A") });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            SetupSut();

            _mockTeletype.Input.Enqueue("10 PRINT");
            _mockTeletype.Input.Enqueue("20 PRINT");
            _mockTeletype.Input.Enqueue("SYSTEM");

            _mockTokeniser.Setup(mt => mt.Tokenise("10 PRINT")).Returns(line10);
            _mockTokeniser.Setup(mt => mt.Tokenise("20 PRINT")).Returns(line20);

            _mockExecutor.SetupSequence(me => me.ExecuteLine())
                .Returns(true);

            _sut.Execute();

            _mockProgramRepository.Verify(s => s.SetProgramLine(line10), Times.Once);
            _mockProgramRepository.Verify(s => s.SetProgramLine(line20), Times.Once);
        }

        /// <summary>
        /// Check interpreter handles end exception nicely.
        /// </summary>
        [TestMethod]
        public void InterpreterHandlesEndCorrectly()
        {
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });
            var runLine = new ProgramLine(null, new List<IToken> { new Token("A") });

            SetupSut();

            _mockTeletype.Input.Enqueue("20 PRINT");
            _mockTeletype.Input.Enqueue("RUN");
            _mockTeletype.Input.Enqueue("SYSTEM");

            _mockTokeniser.Setup(mt => mt.Tokenise("20 PRINT")).Returns(line20);
            _mockTokeniser.Setup(mt => mt.Tokenise("RUN")).Returns(runLine);

            _mockExecutor.SetupSequence(me => me.ExecuteLine())
                .Throws(new ClassicBasic.Interpreter.Exceptions.EndException())
                .Returns(true);
            _mockRunEnvironment.Setup(mre => mre.CurrentLine).Returns(line20);

            _sut.Execute();

            // Input of line 20
            CheckForPrompt();

            // Run
            CheckForPrompt();

            // Prompt after END
            CheckForPrompt();

            // Nothing left
            Assert.AreEqual(0, _mockTeletype.Output.Count);
        }

        /// <summary>
        /// Check interpreter handles stop exception nicely.
        /// </summary>
        [TestMethod]
        public void InterpreterHandlesStopCorrectly()
        {
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });
            var runLine = new ProgramLine(null, new List<IToken> { new Token("A") });

            SetupSut();

            _mockTeletype.Input.Enqueue("20 PRINT");
            _mockTeletype.Input.Enqueue("RUN");
            _mockTeletype.Input.Enqueue("SYSTEM");

            _mockTokeniser.Setup(mt => mt.Tokenise("20 PRINT")).Returns(line20);
            _mockTokeniser.Setup(mt => mt.Tokenise("RUN")).Returns(runLine);

            var breakException = new ClassicBasic.Interpreter.Exceptions.BreakException();
            _mockExecutor.SetupSequence(me => me.ExecuteLine())
                .Throws(breakException)
                .Returns(true);
            _mockRunEnvironment.Setup(mre => mre.CurrentLine).Returns(line20);

            _sut.Execute();

            // Input of line 20
            CheckForPrompt();

            // Run
            CheckForPrompt();

            CheckForError(breakException.ErrorMessage + " IN 20");

            // Prompt after STOP
            CheckForPrompt();

            // Nothing left
            Assert.AreEqual(0, _mockTeletype.Output.Count);
        }

        /// <summary>
        /// Check interpreter handles syntax exception in code nicely.
        /// </summary>
        [TestMethod]
        public void InterpreterHandlesSyntaxError()
        {
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });
            var runLine = new ProgramLine(null, new List<IToken> { new Token("A") });

            SetupSut();

            _mockTeletype.Input.Enqueue("20 PRINT");
            _mockTeletype.Input.Enqueue("RUN");
            _mockTeletype.Input.Enqueue("SYSTEM");

            _mockTokeniser.Setup(mt => mt.Tokenise("20 PRINT")).Returns(line20);
            _mockTokeniser.Setup(mt => mt.Tokenise("RUN")).Returns(runLine);

            var syntaxException = new ClassicBasic.Interpreter.Exceptions.SyntaxErrorException();
            _mockExecutor.SetupSequence(me => me.ExecuteLine())
                .Throws(syntaxException)
                .Returns(true);
            _mockRunEnvironment.Setup(mre => mre.CurrentLine).Returns(line20);

            _sut.Execute();

            // Input of line 20
            CheckForPrompt();

            // Run
            CheckForPrompt();

            CheckForError("?" + syntaxException.ErrorMessage + " ERROR IN 20");

            // Prompt after STOP
            CheckForPrompt();

            // Nothing left
            Assert.AreEqual(0, _mockTeletype.Output.Count);
        }

        /// <summary>
        /// Check interpreter handles syntax exception in immediate nicely.
        /// </summary>
        [TestMethod]
        public void InterpreterHandlesImmediateSyntaxError()
        {
            var runLine = new ProgramLine(null, new List<IToken> { new Token("A") });

            SetupSut();

            _mockTeletype.Input.Enqueue("RUN");
            _mockTeletype.Input.Enqueue("SYSTEM");

            _mockTokeniser.Setup(mt => mt.Tokenise("RUN")).Returns(runLine);

            var syntaxException = new ClassicBasic.Interpreter.Exceptions.SyntaxErrorException();
            _mockExecutor.SetupSequence(me => me.ExecuteLine())
                .Throws(syntaxException)
                .Returns(true);
            _mockRunEnvironment.Setup(mre => mre.CurrentLine).Returns(runLine);

            _sut.Execute();

            // Run
            CheckForPrompt();

            CheckForError("?" + syntaxException.ErrorMessage + " ERROR");

            // Prompt after STOP
            CheckForPrompt();

            // Nothing left
            Assert.AreEqual(0, _mockTeletype.Output.Count);
        }

        /// <summary>
        /// Check interpreter handles syntax exception from tokeniser nicely.
        /// </summary>
        [TestMethod]
        public void InterpreterHandlesTokeniserSyntaxError()
        {
            SetupSut();

            _mockTeletype.Input.Enqueue("100000 PRINT");
            _mockTeletype.Input.Enqueue("SYSTEM");

            var syntaxException = new ClassicBasic.Interpreter.Exceptions.SyntaxErrorException();
            _mockTokeniser.Setup(mt => mt.Tokenise("100000 PRINT"))
                .Throws(syntaxException);

            _mockRunEnvironment.Setup(mre => mre.CurrentLine).Returns((ProgramLine)null);

            _sut.Execute();

            // Run
            CheckForPrompt();

            CheckForError("?" + syntaxException.ErrorMessage + " ERROR");

            // Prompt after Syntax error
            CheckForPrompt();

            // Nothing left
            Assert.AreEqual(0, _mockTeletype.Output.Count);
        }


        /// <summary>
        /// Check interpreter handles break immediate nicely.
        /// </summary>
        [TestMethod]
        public void InterpreterHandlesBreakBeingThrownByRead()
        {
            var runLine = new ProgramLine(null, new List<IToken> { new Token("A") });

            SetupSut();

            _mockTeletype.Input.Enqueue("BREAK");
            _mockTeletype.Input.Enqueue("SYSTEM");

            _mockTokeniser.Setup(mt => mt.Tokenise("RUN")).Returns(runLine);

            _mockTeletype.CancelEventHandler += MockTeletype_CancelEventHandler;

            _sut.Execute();

            // Run
            CheckForPrompt();

            // Prompt after STOP
            CheckForPrompt();

            // Nothing left
            Assert.AreEqual(0, _mockTeletype.Output.Count);
        }

        private void MockTeletype_CancelEventHandler(object sender, ConsoleCancelEventArgs e)
        {
            _mockRunEnvironment.Object.KeyboardBreak = true;
        }

        private void SetupSut()
        {
            _mockTeletype = new MockTeletype();
            _mockTokeniser = new Mock<ITokeniser>();
            _mockRunEnvironment = new Mock<IRunEnvironment>();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _mockExecutor = new Mock<IExecutor>();

            // Make default return true so executor returns back.
            _mockExecutor.SetReturnsDefault(true);

            var system = new ProgramLine(null, new List<IToken> { new Token("A") });
            _mockTokeniser.Setup(mt => mt.Tokenise("SYSTEM")).Returns(system);

            _sut = new Interpreter(
                new TeletypeWithPosition(_mockTeletype),
                _mockTokeniser.Object,
                _mockRunEnvironment.Object,
                _mockProgramRepository.Object,
                _mockExecutor.Object);
        }

        private void CheckForPrompt()
        {
            Assert.AreEqual(Environment.NewLine, _mockTeletype.Output.Dequeue());
            Assert.AreEqual(">", _mockTeletype.Output.Dequeue());
        }

        private void CheckForError(string message)
        {
            Assert.AreEqual(Environment.NewLine, _mockTeletype.Output.Dequeue());
            Assert.AreEqual(message, _mockTeletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _mockTeletype.Output.Dequeue());
        }
    }
}
