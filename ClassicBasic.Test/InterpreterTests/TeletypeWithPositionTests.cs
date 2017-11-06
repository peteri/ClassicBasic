// <copyright file="TeletypeWithPositionTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the Teletype with position
    /// </summary>
    [TestClass]
    public class TeletypeWithPositionTests
    {
        private Mock<ITeletype> _mockTeletype;
        private ITeletypeWithPosition _sut;

        /// <summary>
        /// Converts newline to a write of Environment.newline and resets cursor position.
        /// </summary>
        [TestMethod]
        public void TeletypeWithPositionWritesEnvironmentNewLine()
        {
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.Width).Returns(80);

            _sut = new TeletypeWithPosition(_mockTeletype.Object);

            _sut.NewLine();

            _mockTeletype.Verify(mt => mt.Write(Environment.NewLine), Times.Once);
            Assert.AreEqual(1, _sut.Position());
        }

        /// <summary>
        /// TeletypeWithPosition Forwards writes to underlying teletype.
        /// </summary>
        [TestMethod]
        public void TeletypeWithPositionForwardsWrites()
        {
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.Width).Returns(80);
            _sut = new TeletypeWithPosition(_mockTeletype.Object);

            _sut.Write("HELLO");

            _mockTeletype.Verify(mt => mt.Write("HELLO"), Times.Once);
            Assert.AreEqual(6, _sut.Position());
        }

        /// <summary>
        /// TeletypeWithPosition Forwards ReadChar to underlying teletype.
        /// </summary>
        [TestMethod]
        public void TeletypeWithPositionForwardsReadChar()
        {
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.ReadChar()).Returns('A');
            _sut = new TeletypeWithPosition(_mockTeletype.Object);

            var readChar = _sut.ReadChar();

            _mockTeletype.Verify(mt => mt.ReadChar(), Times.Once);
            Assert.AreEqual('A', readChar);
        }

        /// <summary>
        /// TeletypeWithPosition Forwards reads to underlying teletype.
        /// And resets position.
        /// </summary>
        [TestMethod]
        public void TeletypeWithPositionForwardsReads()
        {
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.Width).Returns(80);
            _sut = new TeletypeWithPosition(_mockTeletype.Object);
            _mockTeletype.Setup(mt => mt.Read()).Returns("RUN");

            _sut.Write(">");
            Assert.AreNotEqual(1, _sut.Position());
            var result = _sut.Read();

            Assert.AreEqual("RUN", result);
            Assert.AreEqual(1, _sut.Position());
        }

        /// <summary>
        /// Test operation of commas
        /// </summary>
        /// <param name="beforeSpaceCount">Number of spaces to print</param>
        /// <param name="comma">Output a comma or not.</param>
        /// <param name="output">string to output.</param>
        /// <param name="expectedBefore">Value before comma and string.</param>
        /// <param name="expectedAfter">Value after comma and string.</param>
        [DataTestMethod]
        [DataRow(0, true, "", 1, 15)]
        [DataRow(7, true, "", 8, 15)]
        [DataRow(14, true, "", 15, 29)]
        [DataRow(28, true, "", 29, 43)]
        [DataRow(42, true, "", 43, 57)]
        [DataRow(56, true, "", 57, 1)]
        [DataRow(0, true, "0123456789ABC", 1, 15)]
        [DataRow(14, true, "0123456789ABC", 15, 29)]
        [DataRow(0, true, "0123456789ABCD", 1, 29)]
        [DataRow(14, true, "0123456789ABCD", 15, 43)]
        public void TeletypeWithPositionCommasInCorrectPlace(
            int beforeSpaceCount,
            bool comma,
            string output,
            int expectedBefore,
            int expectedAfter)
        {
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.Width).Returns(80);

            _sut = new TeletypeWithPosition(_mockTeletype.Object);
            _sut.Space((short)beforeSpaceCount);
            var actualBefore = _sut.Position();
            _sut.Write(output);
            if (comma)
            {
                _sut.NextComma();
            }

            var actualAfter = _sut.Position();
            Assert.AreEqual(expectedBefore, actualBefore);
            Assert.AreEqual(expectedAfter, actualAfter);
        }

        /// <summary>
        /// Checks for out of range exceptions in spc.
        /// </summary>
        /// <param name="parameter">count to pass.</param>
        /// <param name="throwsException">true if we expected exception.</param>
        [DataTestMethod]
        [DataRow(-1, true)]
        [DataRow(0, false)]
        [DataRow(1, false)]
        [DataRow(255, false)]
        [DataRow(256, true)]
        public void TestOutOfRangeParametersToSpc(int parameter, bool throwsException)
        {
            bool exceptionThrown = false;
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.Width).Returns(80);

            try
            {
                _sut = new TeletypeWithPosition(_mockTeletype.Object);
                _sut.Space((short)parameter);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown);
        }

        /// <summary>
        /// Checks for out of range exceptions in tab.
        /// </summary>
        /// <param name="parameter">count to pass.</param>
        /// <param name="throwsException">true if we expected exception.</param>
        [DataTestMethod]
        [DataRow(-1, true)]
        [DataRow(0, false)]
        [DataRow(1, false)]
        [DataRow(255, false)]
        [DataRow(256, true)]
        public void TestOutOfRangeParametersToTab(int parameter, bool throwsException)
        {
            bool exceptionThrown = false;
            _mockTeletype = new Mock<ITeletype>();
            _mockTeletype.Setup(mt => mt.Width).Returns(80);

            try
            {
                _sut = new TeletypeWithPosition(_mockTeletype.Object);
                _sut.Tab((short)parameter);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown);
        }
    }
}
