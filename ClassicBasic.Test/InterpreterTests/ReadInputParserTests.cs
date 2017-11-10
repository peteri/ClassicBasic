// <copyright file="ReadInputParserTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test the ReadInputParser
    /// </summary>
    [TestClass]
    public class ReadInputParserTests
    {
        private ReadInputParser _sut;
        private Queue<string> _inputQueue;

        /// <summary>
        /// Do something.
        /// </summary>
        [TestMethod]
        public void ReadInputParserDoesThings()
        {
            SetupSut();
            Assert.Inconclusive();
        }

        private void SetupSut()
        {
            _inputQueue = new Queue<string>();
            _sut = new ReadInputParser(() => _inputQueue.Count == 0 ? null : _inputQueue.Dequeue());
        }
    }
}
