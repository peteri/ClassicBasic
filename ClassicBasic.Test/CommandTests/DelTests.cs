// <copyright file="DelTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class for the DEL statement.
    /// </summary>
    [TestClass]
    public class DelTests
    {
        /// <summary>
        /// Quick multi function test for delete.
        /// </summary>
        /// <param name="start">Start line number.</param>
        /// <param name="hasComma">Has a comma in the line.</param>
        /// <param name="end">End line number.</param>
        /// <param name="throwsException">Is going to throw syntax error.</param>
        [DataTestMethod]
        [DataRow(10, true, 30, false)]
        [DataRow(10, true, null, true)]
        [DataRow(null, true, 30, true)]
        [DataRow(null, true, null, true)]
        [DataRow(10, false, 40, true)]
        public void DeleteTestsFromImmediateMode(int? start, bool hasComma, int? end, bool throwsException)
        {
            var mockProgramRepository = new Mock<IProgramRepository>();
            var tokens = new List<IToken>();
            if (start.HasValue)
            {
                tokens.Add(new Token(start.Value.ToString()));
            }

            if (hasComma)
            {
                tokens.Add(new Token(",", TokenClass.Seperator, TokenType.Comma));
            }

            if (end.HasValue)
            {
                tokens.Add(new Token(end.Value.ToString()));
            }

            var runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(null, tokens)
            };

            var sut = new Del(runEnvironment, mockProgramRepository.Object);
            var exceptionThrown = false;
            try
            {
                sut.Execute();
                mockProgramRepository.Verify(mpr => mpr.DeleteProgramLines(start.Value, end.Value));
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown);
        }

        /// <summary>
        /// Delete ends program and clears continue.
        /// </summary>
        [TestMethod]
        public void DeleteEndsProgramFromDeferredMode()
        {
            var mockProgramRepository = new Mock<IProgramRepository>();
            var tokens = new List<IToken>
            {
                new Token("30"),
                new Token(",", TokenClass.Seperator, TokenType.Comma),
                new Token("40")
            };

            var runEnvironment = new RunEnvironment
            {
                CurrentLine = new ProgramLine(10, tokens),
                ContinueLineNumber = 10
            };

            var sut = new Del(runEnvironment, mockProgramRepository.Object);
            var exceptionThrown = false;
            try
            {
                sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.EndException)
            {
                exceptionThrown = true;
            }

            mockProgramRepository.Verify(mpr => mpr.DeleteProgramLines(30, 40));
            Assert.IsNull(runEnvironment.ContinueLineNumber);
            Assert.IsTrue(exceptionThrown);
        }
    }
}
