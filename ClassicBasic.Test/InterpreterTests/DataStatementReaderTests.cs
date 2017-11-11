// <copyright file="DataStatementReaderTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the data statement reader.
    /// </summary>
    [TestClass]
    public class DataStatementReaderTests
    {
        private DataStatementReader _sut;
        private Mock<IProgramRepository> _mockProgramRepository;
        private IVariableRepository _variableRepository;
        private VariableReference[] _numericVariables = new VariableReference[3];
        private VariableReference[] _stringVariables = new VariableReference[3];

#warning Need test for error line number

        /// <summary>
        /// Data statement reader throws when no data in program.
        /// </summary>
        [TestMethod]
        public void DataStatementThrowsWhenNoDataInProgram()
        {
            SetupSut();
            bool exceptionThrown = false;

            try
            {
                _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
            }
            catch (ClassicBasic.Interpreter.Exceptions.OutOfDataException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Data statement reader throws when no more data in program.
        /// </summary>
        [TestMethod]
        public void DataStatementThrowsWhenNoMoreDataInProgram()
        {
            SetupSut();
            bool exceptionThrown = false;

            _mockProgramRepository.Setup(mpr => mpr.GetFirstLine())
                .Returns(new ProgramLine(10, new List<IToken> { new Token("1,2", TokenType.ClassData) }));

            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10))
                .Returns(new ProgramLine(
                    20,
                    new List<IToken> { new Token("3,4", TokenType.ClassRemark), new Token("3,4", TokenType.ClassData) }));

            try
            {
                for (double i = 1.0; i <= 5.0; i++)
                {
                    _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                    Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
                }
            }
            catch (ClassicBasic.Interpreter.Exceptions.OutOfDataException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Data statement restore with no line number resets to start of program.
        /// </summary>
        [TestMethod]
        public void DataStatementRestoreWithNoLineNumberResetsToStartOfProgram()
        {
            SetupSut();

            _mockProgramRepository.Setup(mpr => mpr.GetFirstLine())
                .Returns(() => new ProgramLine(
                    10,
                    new List<IToken> { new Token("1,2", TokenType.ClassData), new Token("XXX", TokenType.ClassRemark) }));

            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10))
                .Returns(() => new ProgramLine(
                    20,
                    new List<IToken> { new Token("XXX", TokenType.ClassRemark), new Token("3,4", TokenType.ClassData) }));

            for (double i = 1.0; i <= 4.0; i++)
            {
                _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
            }

            _sut.RestoreToLineNumber(null);

            for (double i = 1.0; i <= 4.0; i++)
            {
                _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
            }
        }

        /// <summary>
        /// Data statement throws out of data until restore called.
        /// </summary>
        [TestMethod]
        public void DataStatementThrowsOutOfDataUntilRestoreCalled()
        {
            SetupSut();

            _mockProgramRepository.Setup(mpr => mpr.GetFirstLine())
                .Returns(() => new ProgramLine(
                    10,
                    new List<IToken> { new Token("1,2", TokenType.ClassData), new Token("XXX", TokenType.ClassRemark) }));

            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10))
                .Returns(() => new ProgramLine(
                    20,
                    new List<IToken> { new Token("XXX", TokenType.ClassRemark), new Token("3,4", TokenType.ClassData) }));

            int exceptionCount = 0;
            for (double i = 1.0; i <= 7.0; i++)
            {
                try
                {
                    _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                    Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
                }
                catch (ClassicBasic.Interpreter.Exceptions.OutOfDataException)
                {
                    exceptionCount++;
                }
            }

            Assert.AreEqual(3, exceptionCount);

            _sut.RestoreToLineNumber(null);

            for (double i = 1.0; i <= 4.0; i++)
            {
                _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
            }
        }

        /// <summary>
        /// Data statement restore with no line number resets to line number.
        /// </summary>
        [TestMethod]
        public void DataStatementRestoreWithLineNumberResetsToLineNumber()
        {
            SetupSut();

            _mockProgramRepository.Setup(mpr => mpr.GetFirstLine())
                .Returns(() => new ProgramLine(
                    10,
                    new List<IToken> { new Token("1,2", TokenType.ClassData), new Token("XXX", TokenType.ClassRemark) }));

            _mockProgramRepository.Setup(mpr => mpr.GetNextLine(10))
                .Returns(() => new ProgramLine(
                    20,
                    new List<IToken> { new Token("XXX", TokenType.ClassRemark), new Token("3,4", TokenType.ClassData) }));

            _mockProgramRepository.Setup(mpr => mpr.GetLine(20))
                .Returns(() => new ProgramLine(
                    20,
                    new List<IToken> { new Token("XXX", TokenType.ClassRemark), new Token("3,4", TokenType.ClassData) }));

            for (double i = 1.0; i <= 4.0; i++)
            {
                _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
            }

            _sut.RestoreToLineNumber(20);

            for (double i = 3.0; i <= 4.0; i++)
            {
                _sut.ReadInputParser.ReadVariables(new List<VariableReference> { _numericVariables[0] });
                Assert.AreEqual(i, _numericVariables[0].GetValue().ValueAsDouble());
            }
        }

        private void SetupSut()
        {
            _mockProgramRepository = new Mock<IProgramRepository>();
            _sut = new DataStatementReader(_mockProgramRepository.Object);
            _variableRepository = new VariableRepository();
            _numericVariables[0] = _variableRepository.GetOrCreateVariable("A", new short[] { });
            _numericVariables[1] = _variableRepository.GetOrCreateVariable("B", new short[] { });
            _numericVariables[2] = _variableRepository.GetOrCreateVariable("C", new short[] { });
            _stringVariables[0] = _variableRepository.GetOrCreateVariable("A$", new short[] { });
            _stringVariables[1] = _variableRepository.GetOrCreateVariable("B$", new short[] { });
            _stringVariables[2] = _variableRepository.GetOrCreateVariable("C$", new short[] { });
        }
    }
}
