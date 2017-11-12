// <copyright file="OnErrTests.cs" company="Peter Ibbotson">
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
    /// Test class for ON ERR.
    /// </summary>
    [TestClass]
    public class OnErrTests
    {
        private IRunEnvironment _runEnvironment;
        private OnErr _sut;

        /// <summary>
        /// ON ERR sets the error line handler.
        /// </summary>
        public void OnErrSetsErrorLine()
        {
            var tokens = new List<IToken> { new Token("1000") };
            _runEnvironment.CurrentLine = new ProgramLine(20, tokens);
            _sut.Execute();
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _sut = new OnErr(_runEnvironment);
        }
    }
}
