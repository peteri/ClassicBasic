﻿// <copyright file="RemarkTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ClassicBasic.Interpreter.Commands;

    /// <summary>
    /// Tests the REM statement
    /// </summary>
    [TestClass]
    public class RemarkTests
    {
        /// <summary>
        /// Tests REM eats the next token
        /// </summary>
        [TestMethod]
        public void RemarkEatsNextToken()
        {
            var runEnvironment = new RunEnvironment();
            var programLine = new ProgramLine(10, new List<IToken> { new Token("Hello Mum", TokenType.ClassRemark) });
            runEnvironment.CurrentLine = programLine;
            var sut = new Remark(runEnvironment);
            sut.Execute();
            Assert.IsTrue(runEnvironment.CurrentLine.EndOfLine);
        }
    }
}