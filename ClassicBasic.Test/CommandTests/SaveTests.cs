// <copyright file="SaveTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using System.IO.Abstractions.TestingHelpers;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests GOTO
    /// </summary>
    [TestClass]
    public class SaveTests
    {
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private IProgramRepository _programRepository;
        private IRunEnvironment _runEnvironment;
        private MockFileSystem _mockFileSystem;
        private Save _sut;

        /// <summary>
        /// Test the save command.
        /// </summary>
        [TestMethod]
        public void SaveSavesProgram()
        {
            SetupSut();
            var tokens = new List<IToken>();
            var currentLine = new ProgramLine(null, tokens);
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(@"c:\test.bas"));
            _runEnvironment.CurrentLine = currentLine;
            _sut.Execute();
            var linesInFile = _mockFileSystem.File.ReadAllLines(@"c:\test.bas");
            Assert.AreEqual(4, linesInFile.Length);
            Assert.AreEqual("10 ONE", linesInFile[0]);
            Assert.AreEqual("20 TWO", linesInFile[1]);
            Assert.AreEqual("30 THREE", linesInFile[2]);
            Assert.AreEqual("40 FOUR", linesInFile[3]);
        }

        /// <summary>
        /// Test the save command.
        /// </summary>
        [TestMethod]
        public void SaveReportsExceptionDuringSaveProgram()
        {
            SetupSut();
            var tokens = new List<IToken>();
            var currentLine = new ProgramLine(null, tokens);
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(@"c:\temp\tes<t.bas"));
            _runEnvironment.CurrentLine = currentLine;
            var exception = Test.Throws<BasicException>(_sut.Execute);
            Assert.IsTrue(exception.ErrorMessage.Contains("BAD SAVE Illegal characters"));
        }

        private void SetupSut()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _programRepository = new ProgramRepository();
            _runEnvironment = new RunEnvironment();
            _mockFileSystem = new MockFileSystem();
            _mockFileSystem.Directory.CreateDirectory("c:\\");
            _programRepository.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("ONE") }));
            _programRepository.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("TWO") }));
            _programRepository.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("THREE") }));
            _programRepository.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("FOUR") }));
            _sut = new Save(_mockExpressionEvaluator.Object, _programRepository, _mockFileSystem);
        }
    }
}
