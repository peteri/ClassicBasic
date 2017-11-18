﻿// <copyright file="VariableReferenceTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests variable references.
    /// </summary>
    [TestClass]
    public class VariableReferenceTests
    {
        /// <summary>
        /// Can write and read to a double variable.
        /// </summary>
        [TestMethod]
        public void CanWriteAndReadToDoubleVariable()
        {
            var variable = new Variable("A", new short[] { });
            var sut = new VariableReference(variable, new short[] { });
            sut.SetValue(new Accumulator(40000.25));
            Assert.AreEqual(40000.25, sut.GetValue().ValueAsDouble());
            Assert.IsFalse(sut.IsString);
        }

        /// <summary>
        /// Can write to a short variable.
        /// </summary>
        [TestMethod]
        public void CanWriteAndReadToShortVariable()
        {
            var variable = new Variable("A%", new short[] { });
            var sut = new VariableReference(variable, new short[] { });
            sut.SetValue(new Accumulator(-3000.0));
            Assert.AreEqual(-3000, sut.GetValue().ValueAsShort());
            Assert.IsFalse(sut.IsString);
        }

        /// <summary>
        /// Can write to a string variable.
        /// </summary>
        [TestMethod]
        public void CanWriteAndReadToStringVariable()
        {
            var variable = new Variable("A$", new short[] { });
            var sut = new VariableReference(variable, new short[] { });
            sut.SetValue(new Accumulator("HELLO"));
            Assert.AreEqual("HELLO", sut.GetValue().ValueAsString());
            Assert.IsTrue(sut.IsString);
        }

        /// <summary>
        /// Throws an exception when to out of range value is written to a short variable.
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow(-32769.0, true)]
        [DataRow(-32768.0, false)]
        [DataRow(4.5, false)]
        [DataRow(32767.0, false)]
        [DataRow(32768.0, true)]
        public void ThrowsWhenWritingBigValueToShortVariable(double value, bool throwsException)
        {
            var variable = new Variable("A%", new short[] { });
            var sut = new VariableReference(variable, new short[] { });
            Test.Throws<IllegalQuantityException>(() => sut.SetValue(new Accumulator(value)), throwsException);
        }

        /// <summary>
        /// Can read and write to a double array variable.
        /// </summary>
        [TestMethod]
        public void CanWriteAndReadToDoubleArrayVariable()
        {
            var variable = new Variable("A", new short[] { 7, 7 });
            var sut = new VariableReference(variable, new short[] { 4, 4 });
            sut.SetValue(new Accumulator(40000.25));
            Assert.AreEqual(40000.25, sut.GetValue().ValueAsDouble());
            Assert.IsFalse(sut.IsString);
        }

        /// <summary>
        /// Can read and write to a short array variable.
        /// </summary>
        [TestMethod]
        public void CanWriteAndReadToShortArrayVariable()
        {
            var variable = new Variable("A%", new short[] { 6, 6 });
            var sut = new VariableReference(variable, new short[] { 2, 2 });
            sut.SetValue(new Accumulator(-3000.0));
            Assert.AreEqual(-3000, sut.GetValue().ValueAsShort());
            Assert.IsFalse(sut.IsString);
        }

        /// <summary>
        /// Can read and write to a string array variable.
        /// </summary>
        [TestMethod]
        public void CanWriteAndReadToStringArrayVariable()
        {
            var variable = new Variable("A$", new short[] { 5, 5 });
            var sut = new VariableReference(variable, new short[] { 3, 3 });
            sut.SetValue(new Accumulator("HELLO"));
            Assert.AreEqual("HELLO", sut.GetValue().ValueAsString());
            Assert.IsTrue(sut.IsString);
        }

        /// <summary>
        /// Get back empty string for uninitialised array element.
        /// </summary>
        [TestMethod]
        public void CanReadUninitializedStringArrayVariable()
        {
            var variable = new Variable("A$", new short[] { 5, 5 });
            var sut = new VariableReference(variable, new short[] { 3, 3 });
            Assert.AreEqual(string.Empty, sut.GetValue().ValueAsString());
        }
    }
}
