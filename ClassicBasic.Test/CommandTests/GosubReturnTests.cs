// <copyright file="GosubReturnTests.cs" company="Peter Ibbotson">
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
    /// Tests gosub and return.
    /// </summary>
    [TestClass]
    public class GosubReturnTests
    {
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IProgramRepository> _mockProgramRepository;
        private IRunEnvironment _runEnvironment;
        private ProgramLine _gosubProgramLine;
        private ProgramLine _targetProgramLine;
        private Gosub _sut;

        /// <summary>
        /// Return pops stack entry and sets current line
        /// </summary>
        [TestMethod]
        public void ReturnPopsStackAndSetsCurrentLineAndToken()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Return(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            sut.Execute();
            Assert.AreEqual(0, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(line10, runEnvironment.CurrentLine);
            Assert.AreEqual(line10, runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Return uses just one return address
        /// </summary>
        [TestMethod]
        public void ReturnUsesOneReturnAddress()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Return(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            sut.Execute();
            Assert.AreEqual(1, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(line10, runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Return uses return address and for next loops.
        /// </summary>
        [TestMethod]
        public void ReturnUsesReturnAddressAndForNextLoops()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Return(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { VariableName = "A", Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { VariableName = "B", Line = line10, LineToken = line10.CurrentToken });
            sut.Execute();
            Assert.AreEqual(1, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(line10, runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Return throws when stack has no return address.
        /// </summary>
        [TestMethod]
        public void ReturnThrowsWhenNoReturnAddress()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Return(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { VariableName = "A", Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { VariableName = "B", Line = line10, LineToken = line10.CurrentToken });
            bool exceptionThrown = false;
            try
            {
                sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.ReturnWithoutGosubException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests GOSUB pushes return address and sets current line to target.
        /// </summary>
        [TestMethod]
        public void GosubPushesReturnAddressAndSetsCurrentLineToTarget()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetLineNumber()).Returns(100);
            _sut.Execute();
            _mockProgramRepository.Verify(mpr => mpr.GetLine(100), Times.Once);
            Assert.AreEqual(100, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var returnAddress = _runEnvironment.ProgramStack.Pop();
            Assert.AreEqual(_gosubProgramLine, returnAddress.Line);
            Assert.AreEqual(2, returnAddress.LineToken);
            Assert.AreEqual(null, returnAddress.VariableName);
            Assert.AreEqual(_targetProgramLine, _runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Tests GOSUB Throws exception if line number does not exist.
        /// </summary>
        [TestMethod]
        public void GosubThrowsExceptionIfLineNumberDoesExist()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetLineNumber()).Returns(110);
            var exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.UndefinedStatementException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Tests GOSUB Throws exception if no line number.
        /// </summary>
        [TestMethod]
        public void GosubThrowsExceptionIfNoLineNumber()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetLineNumber()).Returns((int?)null);
            var exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.UndefinedStatementException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockProgramRepository = new Mock<IProgramRepository>();
            _runEnvironment = new RunEnvironment();
            _gosubProgramLine = new ProgramLine(10, new List<IToken> { });
            _targetProgramLine = new ProgramLine(100, new List<IToken> { });

            _sut = new Gosub(_runEnvironment, _mockExpressionEvaluator.Object, _mockProgramRepository.Object);
            _mockProgramRepository.Setup(mpr => mpr.GetLine(100)).Returns(_targetProgramLine);
            _mockProgramRepository.Setup(mpr => mpr.GetLine(110))
                .Throws(new ClassicBasic.Interpreter.Exceptions.UndefinedStatementException());
            _runEnvironment.CurrentLine = _gosubProgramLine;
            _gosubProgramLine.CurrentToken = 2;
        }
    }
}
