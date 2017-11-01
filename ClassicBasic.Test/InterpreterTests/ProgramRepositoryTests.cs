// <copyright file="ProgramRepositoryTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Program Repository class.
    /// </summary>
    [TestClass]
    public class ProgramRepositoryTests
    {
        /// <summary>
        /// Empty repository returns null.
        /// </summary>
        [TestMethod]
        public void ProgramRepositoryWhenEmptyFirstLineReturnsNull()
        {
            var sut = new ProgramRepository();
            var currentLine = sut.GetFirstLine();
            Assert.IsNull(currentLine);
        }

        /// <summary>
        /// Test clearing the repository.
        /// </summary>
        [TestMethod]
        public void ProgramRepositoryClears()
        {
            var sut = new ProgramRepository();
            sut.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(50, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("X") }));

            Assert.IsNotNull(sut.GetFirstLine());
            sut.Clear();
            Assert.IsNull(sut.GetFirstLine());
        }

        /// <summary>
        /// Program lines are in sorted order.
        /// </summary>
        [TestMethod]
        public void ProgramRepositorySortsLinesIntoOrder()
        {
            var sut = new ProgramRepository();
            sut.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(50, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("X") }));

            var lineNumbers = new List<int?>();
            var currentLine = sut.GetFirstLine();
            do
            {
                lineNumbers.Add(currentLine.LineNumber);
                currentLine = sut.GetNextLine(currentLine.LineNumber.Value);
            }
            while (currentLine != null);

            CollectionAssert.AreEqual(new List<int?> { 10, 20, 30, 40, 50 }, lineNumbers);
        }

        /// <summary>
        /// Program lines are in sorted order.
        /// </summary>
        [TestMethod]
        public void ProgramRepositoryCanDeleteLines()
        {
            var sut = new ProgramRepository();
            sut.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(50, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("X") }));

            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { }));
            sut.SetProgramLine(new ProgramLine(30, new List<IToken> { }));
            sut.SetProgramLine(new ProgramLine(50, new List<IToken> { }));

            var lineNumbers = new List<int?>();
            var currentLine = sut.GetFirstLine();
            do
            {
                lineNumbers.Add(currentLine.LineNumber);
                currentLine = sut.GetNextLine(currentLine.LineNumber.Value);
            }
            while (currentLine != null);

            CollectionAssert.AreEqual(new List<int?> { 20, 40 }, lineNumbers);
        }

        /// <summary>
        /// Program lines are in sorted order.
        /// </summary>
        /// <param name="start">Start line to delete.</param>
        /// <param name="end">End line to delete.</param>
        /// <param name="expected">Expect lines afterwards.</param>
        [DataTestMethod]
        [DataRow(25, 29, new int[] { 10, 20, 30, 40, 50 })]
        [DataRow(5, 20, new int[] { 30, 40, 50 })]
        [DataRow(30, 60, new int[] { 10, 20 })]
        public void ProgramRepositoryCanDeleteLineRange(int start, int end, int[] expected)
        {
            var sut = new ProgramRepository();
            sut.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(50, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("X") }));

            sut.DeleteProgramLines(start, end);

            var lineNumbers = new List<int>();
            var currentLine = sut.GetFirstLine();
            do
            {
                lineNumbers.Add(currentLine.LineNumber.Value);
                currentLine = sut.GetNextLine(currentLine.LineNumber.Value);
            }
            while (currentLine != null);

            CollectionAssert.AreEqual(expected, lineNumbers.ToArray());
        }

        /// <summary>
        /// Program lines can be got with a current token position of zero.
        /// </summary>
        [TestMethod]
        public void ProgramRepositoryCanGetLineResetsCurrentToken()
        {
            var sut = new ProgramRepository();
            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("X") }));

            var line20 = sut.GetLine(20);
            line20.NextToken();

            Assert.IsTrue(line20.EndOfLine);

            line20 = sut.GetLine(20);
            Assert.IsFalse(line20.EndOfLine);
            Assert.AreEqual(0, line20.CurrentToken);
        }

        /// <summary>
        /// Program lines can be got with a current token position of zero.
        /// </summary>
        [TestMethod]
        public void ProgramRepositoryGetLineThrowsOnInvalidLineNumber()
        {
            var exceptionThrown = false;
            var sut = new ProgramRepository();
            sut.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("X") }));
            sut.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("X") }));

            try
            {
                sut.GetLine(25);
            }
            catch (ClassicBasic.Interpreter.Exceptions.UndefinedStatementException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
