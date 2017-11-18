// <copyright file="PopTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the POP command.
    /// </summary>
    [TestClass]
    public class PopTests
    {
        /// <summary>
        /// Pop delete return address
        /// </summary>
        [TestMethod]
        public void PopDeletesReturnAddress()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Pop(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            sut.Execute();
            Assert.AreEqual(0, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(line1000, runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Pop deletes just one return address
        /// </summary>
        [TestMethod]
        public void PopDeletesOneReturnAddress()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Pop(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { Line = line10, LineToken = line10.CurrentToken });
            sut.Execute();
            Assert.AreEqual(1, runEnvironment.ProgramStack.Count);
            Assert.AreEqual(line1000, runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Pop delete return address and for next loops.
        /// </summary>
        [TestMethod]
        public void PopDeletesReturnAddressAndForNextLoops()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Pop(runEnvironment);
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
            Assert.AreEqual(line1000, runEnvironment.CurrentLine);
        }

        /// <summary>
        /// Pop throws when stack has no return address.
        /// </summary>
        [TestMethod]
        public void PopThrowsWhenNoReturnAddress()
        {
            var runEnvironment = new RunEnvironment();
            var sut = new Pop(runEnvironment);
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1000") });
            var line1000 = new ProgramLine(1000, new List<IToken> { });
            line10.NextToken();
            runEnvironment.CurrentLine = line1000;
            runEnvironment.ProgramStack.Push(new StackEntry { VariableName = "A", Line = line10, LineToken = line10.CurrentToken });
            runEnvironment.ProgramStack.Push(new StackEntry { VariableName = "B", Line = line10, LineToken = line10.CurrentToken });
            Test.Throws<ReturnWithoutGosubException>(sut.Execute);
        }
    }
}
