// <copyright file="AccumulatorTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using System;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the accumulator.
    /// </summary>
    [TestClass]
    public class AccumulatorTests
    {
        /// <summary>
        /// Accumulator can be a double
        /// </summary>
        [TestMethod]
        public void AccumulatorInitialiseToDouble()
        {
            var sut = new Accumulator(3.25);
            Assert.AreEqual(typeof(double), sut.Type);
            Assert.AreEqual(3.25, sut.ValueAsDouble());
            Assert.AreEqual("3.25", sut.ToString());
        }

        /// <summary>
        /// Accumulator can be a short
        /// </summary>
        [TestMethod]
        public void AccumulatorInitialiseToShort()
        {
            var sut = new Accumulator((short)-3);
            Assert.AreEqual(typeof(short), sut.Type);
            Assert.AreEqual((short)-3, sut.ValueAsShort());
            Assert.AreEqual("-3", sut.ToString());
        }

        /// <summary>
        /// Accumulator can be a string
        /// </summary>
        [TestMethod]
        public void AccumulatorInitialiseToString()
        {
            var sut = new Accumulator("HELLO");
            Assert.AreEqual(typeof(string), sut.Type);
            Assert.AreEqual("HELLO", sut.ValueAsString());
            Assert.AreEqual("HELLO", sut.ToString());
        }

        /// <summary>
        /// Accumulator can be set to a double
        /// </summary>
        [TestMethod]
        public void AccumulatorSetValueToDouble()
        {
            var sut = new Accumulator(1);
            sut.SetValue(3.25);
            Assert.AreEqual(typeof(double), sut.Type);
            Assert.AreEqual(3.25, sut.ValueAsDouble());
            Assert.AreEqual("3.25", sut.ToString());
        }

        /// <summary>
        /// Accumulator set value to a short
        /// </summary>
        [TestMethod]
        public void AccumulatorSetValueToShort()
        {
            var sut = new Accumulator(0.0);
            sut.SetValue((short)-3);
            Assert.AreEqual(typeof(short), sut.Type);
            Assert.AreEqual((short)-3, sut.ValueAsShort());
            Assert.AreEqual("-3", sut.ToString());
        }

        /// <summary>
        /// Accumulator set value to a string
        /// </summary>
        [TestMethod]
        public void AccumulatorSetValueToString()
        {
            var sut = new Accumulator(0.0);
            sut.SetValue("HELLO");
            Assert.AreEqual(typeof(string), sut.Type);
            Assert.AreEqual("HELLO", sut.ValueAsString());
            Assert.AreEqual("HELLO", sut.ToString());
        }

        /// <summary>
        /// Test GetValue
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <param name="wantedType">Type we want to use.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow((short)1, typeof(short), false)]
        [DataRow((short)1, typeof(double), false)]
        [DataRow((short)1, typeof(string), true)]
        [DataRow((short)1, typeof(int), true)]
        [DataRow(1.0, typeof(short), false)]
        [DataRow(1.0, typeof(double), false)]
        [DataRow(1.0, typeof(string), true)]
        [DataRow(1.0, typeof(int), true)]
        [DataRow("X", typeof(short), true)]
        [DataRow("X", typeof(double), true)]
        [DataRow("X", typeof(string), false)]
        [DataRow("X", typeof(int), true)]
        public void AccumulatorTestTypeMismatch(object value, Type wantedType, bool throwsException)
        {
            var sut = new Accumulator(value);
            var exceptionThrown = false;
            try
            {
                sut.GetValue(wantedType);
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {wantedType} {throwsException}");
        }

        /// <summary>
        /// Test GetValueAsDouble
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <param name="expectedValue">Expected Value.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow((short)1, 1.0, false)]
        [DataRow(2.0, 2.0, false)]
        [DataRow(3, 3.0, true)]
        [DataRow("X", 4.0, true)]
        public void AccumulatorGetValueAsDouble(object value, double expectedValue, bool throwsException)
        {
            var sut = new Accumulator(value);
            var exceptionThrown = false;
            try
            {
                var result = sut.ValueAsDouble();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {expectedValue} {throwsException}");
        }

        /// <summary>
        /// Test GetValueAsShort
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <param name="expectedValue">Expected Value.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow((short)1, (short)1, false)]
        [DataRow(2.0, (short)2, false)]
        [DataRow(3, (short)3, true)]
        [DataRow("X", (short)4, true)]
        public void AccumulatorGetValueAsShort(object value, short expectedValue, bool throwsException)
        {
            var sut = new Accumulator(value);
            var exceptionThrown = false;
            try
            {
                var result = sut.ValueAsShort();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {expectedValue} {throwsException}");
        }

        /// <summary>
        /// Test GetValueAsString
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <param name="expectedValue">Expected Value.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow((short)1, "A", true)]
        [DataRow(2.0, "B", true)]
        [DataRow(3, "C", true)]
        [DataRow("X", "X", false)]
        public void AccumulatorGetValueAsString(object value, string expectedValue, bool throwsException)
        {
            var sut = new Accumulator(value);
            var exceptionThrown = false;
            try
            {
                var result = sut.ValueAsString();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {expectedValue} {throwsException}");
        }

        /// <summary>
        /// Test Double to short throws correctly
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow(32767.0, false)]
        [DataRow(-32768.0, false)]
        [DataRow(32768.0, true)]
        [DataRow(-32769.0, true)]
        public void AccumulatorConvertToShortThrows(double value, bool throwsException)
        {
            var sut = new Accumulator(value);
            var exceptionThrown = false;
            try
            {
                var result = sut.ValueAsShort();
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {throwsException}");
        }

        /// <summary>
        /// Test we can set value to 256 characters.
        /// </summary>
        [TestMethod]
        public void AccumulatorNewWithLongStringDoesNotThrowException()
        {
            AccumulatorNewWithTooLongStringThrowsException(new string('X', 256), false);
        }

        /// <summary>
        /// Test we set value to 257 characters throws exception.
        /// </summary>
        [TestMethod]
        public void AccumulatorNewWithTooLongStringThrowsException()
        {
            AccumulatorNewWithTooLongStringThrowsException(new string('X', 257), true);
        }

        /// <summary>
        /// Test we can set value to 256 characters.
        /// </summary>
        [TestMethod]
        public void AccumulatorSetValueWithLongStringDoesNotThrowException()
        {
            AccumulatorSetValueWithTooLongStringThrowsException(new string('X', 256), false);
        }

        /// <summary>
        /// Test we set value to 257 characters throws exception.
        /// </summary>
        [TestMethod]
        public void AccumulatorSetValueWithTooLongStringThrowsException()
        {
            AccumulatorSetValueWithTooLongStringThrowsException(new string('X', 257), true);
        }

        private void AccumulatorSetValueWithTooLongStringThrowsException(string value, bool throwsException)
        {
            var exceptionThrown = false;
            try
            {
                var sut = new Accumulator(0.0);
                sut.SetValue(value);
            }
            catch (ClassicBasic.Interpreter.Exceptions.StringToLongException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {throwsException}");
        }

        private void AccumulatorNewWithTooLongStringThrowsException(string value, bool throwsException)
        {
            var exceptionThrown = false;
            try
            {
                var sut = new Accumulator(value);
            }
            catch (ClassicBasic.Interpreter.Exceptions.StringToLongException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown, $"Parameters {value} {throwsException}");
        }
    }
}
