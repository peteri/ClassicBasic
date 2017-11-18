// <copyright file="ExecutorTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System;
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the Executor class.
    /// </summary>
    [TestClass]
    public class ExecutorTests
    {
        private MockTeletype _mockTeletype;

        private Mock<IToken> _mockPrintToken;
        private Mock<ICommand> _mockPrintCmd;

        private Mock<IToken> _mockRunToken;
        private Mock<ITokeniserCommand> _mockRunCmd;

        private Mock<IToken> _mockLetToken;
        private Mock<ICommand> _mockLetCmd;

        private Mock<IToken> _mockListToken;
        private Mock<IInterruptableCommand> _mockListCmd;

        private Mock<IToken> _mockSystemToken;

        private Mock<IToken> _mockColonToken;

        private Mock<IToken> _mockNotStatement;

        private Mock<ITokensProvider> _mockTokensProvider;
        private IRunEnvironment _runEnvironment;
        private Mock<IProgramRepository> _mockProgramRepository;
        private IExecutor _sut;

        /// <summary>
        /// Executing the SYSTEM command returns true;
        /// </summary>
        [TestMethod]
        public void ExecutingSystemCommandReturnsTrue()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(null, new List<IToken> { _mockSystemToken.Object });
            var result = _sut.ExecuteLine();
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Check we put a fake let statement if the first item is a variable.
        /// </summary>
        [TestMethod]
        public void ExecutingStatementStartingWithAVariableInsertsLetCommand()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(null, new List<IToken> { new Token("VAR") });
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            _mockLetCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check we call get next line with correct line number once we're done.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramGetsTheNextLineCorrectly()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { new Token("VAR") });
            var line20 = new ProgramLine(20, new List<IToken> { _mockPrintToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns(line20);
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            _mockLetCmd.Verify(c => c.Execute(), Times.Once);
            _mockPrintCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check we call get next line with correct line number once we're done.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramGetsTheNextLineCorrectlyWithColons()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { new Token("VAR"), _mockColonToken.Object, _mockPrintToken.Object });
            var line20 = new ProgramLine(20, new List<IToken> { _mockPrintToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns(line20);
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            _mockLetCmd.Verify(c => c.Execute(), Times.Once);
            _mockPrintCmd.Verify(c => c.Execute(), Times.Exactly(2));
        }

        /// <summary>
        /// Check we skip leading colons.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramSkipsLeadingColons()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockColonToken.Object, _mockColonToken.Object, _mockPrintToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            _mockPrintCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check we're happy in the middle of colons.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramSkipsLeadingAndTrailingColons()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockColonToken.Object, _mockPrintToken.Object, _mockColonToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            _mockPrintCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check we skip trailing colons.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramSkipsTrailingColons()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockPrintToken.Object, _mockColonToken.Object, _mockColonToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            _mockPrintCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check an else clause does not require a colon, if an IF THEN trueStatement ELSE falseStatemnt.
        /// runs the true statement, then else needs to eat the following false statement.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramElseDoesNotRequireColon()
        {
            var mockElseToken = new Mock<IToken>();
            var mockElseCmd = mockElseToken.As<ICommand>();
            mockElseToken.Setup(met => met.Statement).Returns(TokenType.Else);

            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockPrintToken.Object, mockElseToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            mockElseCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check an  IF THEN trueStatement ELSE falseStatemnt.
        /// runs the true statement.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramIfDoesNotRequireColon()
        {
            var mockIfToken = new Mock<IToken>();
            var mockIfRepeatCmd = mockIfToken.As<IRepeatExecuteCommand>();
            var mockIfCmd = mockIfToken.As<ICommand>();

            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { mockIfToken.Object, _mockPrintToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns<ProgramLine>(null);
            var result = _sut.ExecuteLine();
            Assert.IsFalse(result);
            mockIfCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check we get we a syntax error if first token is not a variable or statement.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramThrowsSyntaxErrorIfFirstTokenIsNotVariableOrStatement()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockNotStatement.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);
            var result = Test.Throws<SyntaxErrorException, bool>(_sut.ExecuteLine, true);
        }

        /// <summary>
        /// Check we get we a syntax error if first token is not a variable or statement.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramThrowsSyntaxErrorIfNextTokenIsNotVariableOrStatement()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockPrintToken.Object, _mockNotStatement.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);
            var result = Test.Throws<SyntaxErrorException, bool>(_sut.ExecuteLine, true);
        }

        /// <summary>
        /// List uses a Setup and Execute approach, runs until execute returns true.
        /// </summary>
        [TestMethod]
        public void ExecutingListCallsSetupOnceAndExecuteMultipleTimes()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockListToken.Object });

            _mockListCmd.SetupSequence(mlc => mlc.Execute())
                .Returns(false)
                .Returns(false)
                .Returns(true);
            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);

            var result = _sut.ExecuteLine();

            Assert.IsFalse(result);
            _mockListCmd.Verify(mlc => mlc.Setup(), Times.Once);
            _mockListCmd.Verify(mlc => mlc.Execute(), Times.Exactly(3));
        }

        /// <summary>
        /// Run takes a tokeniser as a parameter for execute.
        /// </summary>
        [TestMethod]
        public void ExecutingRunCallsSetupOnceAndExecuteMultipleTimes()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockRunToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);

            var result = _sut.ExecuteLine();

            Assert.IsFalse(result);
            _mockRunCmd.Verify(mrc => mrc.Execute(It.IsAny<ITokeniser>()), Times.Once);
        }

        /// <summary>
        /// List uses a Setup and Execute approach, runs until ctrl-c hit.
        /// </summary>
        [TestMethod]
        public void ExecutingListAndHittingBreakStopsExecutionTimes()
        {
            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockListToken.Object });

            int counter = 0;
            _mockListCmd.Setup(mlc => mlc.Execute())
                .Returns(false)
                .Callback(() =>
                {
                    counter++;
                    if (counter > 5)
                    {
                        _mockTeletype.RaiseCancelEvent();
                    }
                });
            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);

            var result = _sut.ExecuteLine();

            Assert.IsFalse(result);
            _mockListCmd.Verify(mlc => mlc.Setup(), Times.Once);
            _mockListCmd.Verify(mlc => mlc.Execute(), Times.Exactly(6));
        }

        /// <summary>
        /// Check user hitting break sets continue point correctly.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramStopsWhenBreakIsHit()
        {
            ConsoleCancelEventArgs cancelEventArgs = null;

            SetupSut();
            var line10 = new ProgramLine(10, new List<IToken> { _mockColonToken.Object, _mockPrintToken.Object, _mockColonToken.Object });

            _runEnvironment.CurrentLine = line10;
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10)).Returns<ProgramLine>(null);
            _mockPrintCmd.Setup(mpc => mpc.Execute()).Callback(() => cancelEventArgs = _mockTeletype.RaiseCancelEvent());
            var result = Test.Throws<BreakException, bool>(_sut.ExecuteLine, true);
            Assert.IsTrue(cancelEventArgs.Cancel);
            Assert.AreEqual(10, _runEnvironment.ContinueLineNumber);
            Assert.AreEqual(1, _runEnvironment.ContinueToken);
            _mockPrintCmd.Verify(c => c.Execute(), Times.Once);
        }

        /// <summary>
        /// Check if we get an error error if first token is not a variable or statement.
        /// </summary>
        [TestMethod]
        public void ExecutingProgramThrowsSyntaxErrorCallsErrorHandlingWhenSet()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                _mockPrintToken.Object,
                _mockColonToken.Object,
                _mockPrintToken.Object,
                _mockNotStatement.Object
            };

            var line10 = new ProgramLine(10, tokens);
            _runEnvironment.CurrentLine = line10;
            _runEnvironment.OnErrorGotoLineNumber = 30;
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _runEnvironment.ProgramStack.Push(new StackEntry());
            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(20)).Returns<ProgramLine>(null);
            _mockProgramRepository.Setup(mpr => mpr.GetLine(30))
                .Returns(new ProgramLine(30, new List<IToken> { _mockPrintToken.Object }));

            var result = _sut.ExecuteLine();
            Assert.AreEqual(10, _runEnvironment.LastErrorLine);
            Assert.AreEqual(2, _runEnvironment.LastErrorNumber);
            Assert.AreEqual(3, _runEnvironment.LastErrorStackCount);
            Assert.AreEqual(2, _runEnvironment.LastErrorToken);
        }

        private void SetupSut()
        {
            _mockTeletype = new MockTeletype();

            // Use a real RunEnvironment
            _runEnvironment = new RunEnvironment();

            _mockListToken = new Mock<IToken>();
            _mockListToken.Setup(mlt => mlt.Statement).Returns(TokenType.Unknown);
            _mockListToken.Setup(mlt => mlt.TokenClass).Returns(TokenClass.Statement);
            _mockListCmd = _mockListToken.As<IInterruptableCommand>();

            _mockRunToken = new Mock<IToken>();
            _mockRunToken.Setup(mrt => mrt.TokenClass).Returns(TokenClass.Statement);
            _mockRunToken.Setup(mrt => mrt.Statement).Returns(TokenType.Unknown);
            _mockRunCmd = _mockRunToken.As<ITokeniserCommand>();

            _mockLetToken = new Mock<IToken>();
            _mockLetToken.Setup(mlt => mlt.Statement).Returns(TokenType.Let);
            _mockLetCmd = _mockLetToken.As<ICommand>();
            _mockLetCmd.Setup(mlt => mlt.Execute())
                .Callback(() => _runEnvironment.CurrentLine.NextToken());

            _mockPrintToken = new Mock<IToken>();
            _mockPrintToken.Setup(mpt => mpt.TokenClass).Returns(TokenClass.Statement);
            _mockPrintCmd = _mockPrintToken.As<ICommand>();

            _mockSystemToken = new Mock<IToken>();
            _mockSystemToken.Setup(mst => mst.Statement).Returns(TokenType.System);

            _mockColonToken = new Mock<IToken>();
            _mockColonToken.Setup(mct => mct.Seperator).Returns(TokenType.Colon);

            _mockNotStatement = new Mock<IToken>();
            _mockNotStatement.Setup(mns => mns.TokenClass).Returns(TokenClass.Unknown);
            _mockNotStatement.Setup(mns => mns.Seperator).Returns(TokenType.Unknown);
            _mockNotStatement.Setup(mns => mns.Statement).Returns(TokenType.Unknown);

            _mockTokensProvider = new Mock<ITokensProvider>();
            _mockTokensProvider.Setup(mtp => mtp.Tokens).Returns(new List<IToken>
            {
                _mockLetToken.Object
            });

            _mockProgramRepository = new Mock<IProgramRepository>();
            _sut = new Executor(
                _mockTeletype,
                _runEnvironment,
                _mockProgramRepository.Object,
                _mockTokensProvider.Object,
                Mock.Of<ITokeniser>());
        }
    }
}
