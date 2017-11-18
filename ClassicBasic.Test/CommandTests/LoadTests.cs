// <copyright file="LoadTests.cs" company="Peter Ibbotson">
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
    /// Test class for LOAD.
    /// </summary>
    [TestClass]
    public class LoadTests
    {
        private Mock<ITokeniser> _mockTokeniser;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<IProgramRepository> _mockProgramRepository;
        private MockFileSystem _mockFileSystem;
        private Load _sut;
        private ProgramLine _line10;
        private ProgramLine _line20;
        private ProgramLine _line30;

        /// <summary>
        /// Load can load.
        /// </summary>
        [TestMethod]
        public void LoadLoadsCode()
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(@"c:\myfile.bas"));
            _sut.Execute(_mockTokeniser.Object);
            _mockProgramRepository.Verify(mpr => mpr.SetProgramLine(_line10), Times.Once);
            _mockProgramRepository.Verify(mpr => mpr.SetProgramLine(_line20), Times.Once);
            _mockProgramRepository.Verify(mpr => mpr.SetProgramLine(_line30), Times.Once);
        }

        /// <summary>
        /// Load reports file open exception.
        /// </summary>
        /// <param name="fileName">File name to check.</param>
        /// <param name="throwsException">Flag if it thorws exception.</param>
        [DataTestMethod]
        [DataRow(@"c:\myfile.bas", false)]
        [DataRow(@"c:\myfile1.bas", true)]
        public void LoadReportsExceptionCode(string fileName, bool throwsException)
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(fileName));
            BasicException exception = Test.Throws<BasicException>(() => _sut.Execute(_mockTokeniser.Object), throwsException);
            if (throwsException)
            {
                StringAssert.Contains(exception.ErrorMessage, "Can't find " + fileName);
            }
        }

        /// <summary>
        /// Load reports missing linenumber exception.
        /// </summary>
        /// <param name="fileName">File name to check.</param>
        /// <param name="throwsException">Flag if it thorws exception.</param>
        [DataTestMethod]
        [DataRow(@"c:\myfile.bas", false)]
        [DataRow(@"c:\missing.bas", true)]
        public void LoadReportsMissingLineNumberCode(string fileName, bool throwsException)
        {
            SetupSut();
            _mockExpressionEvaluator.Setup(mee => mee.GetExpression()).Returns(new Accumulator(fileName));
            BasicException exception = Test.Throws<BasicException>(() => _sut.Execute(_mockTokeniser.Object), throwsException);
            if (throwsException)
            {
                StringAssert.Contains(exception.ErrorMessage, "LAST GOOD LINE WAS 20");
            }
        }

        private void SetupSut()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            string fileContents =
@"10 REM LINE 10
20 REM LINE 20
30 REM LINE 30";
            string missingLineContents =
@"10 REM LINE 10
20 REM LINE 20
REM LINE 30";
            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\myfile.bas", new MockFileData(fileContents) },
                { @"c:\missing.bas", new MockFileData(missingLineContents) },
            });
            _mockProgramRepository = new Mock<IProgramRepository>();
            _mockTokeniser = new Mock<ITokeniser>();
            _line10 = new ProgramLine(10, new List<IToken> { });
            _line20 = new ProgramLine(20, new List<IToken> { });
            _line30 = new ProgramLine(30, new List<IToken> { });
            _mockTokeniser.Setup(mt => mt.Tokenise("10 REM LINE 10")).Returns(_line10);
            _mockTokeniser.Setup(mt => mt.Tokenise("20 REM LINE 20")).Returns(_line20);
            _mockTokeniser.Setup(mt => mt.Tokenise("30 REM LINE 30")).Returns(_line30);
            _mockTokeniser.Setup(mt => mt.Tokenise("REM LINE 30")).Returns(new ProgramLine(null, new List<IToken> { }));
            _sut = new Load(_mockExpressionEvaluator.Object, _mockFileSystem, _mockProgramRepository.Object);
        }
    }
}
