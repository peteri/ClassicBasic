// <copyright file="VariableTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the variable class.
    /// </summary>
    [TestClass]
    public class VariableTests
    {
        /// <summary>
        /// Default variable type is a double with a value of zero
        /// </summary>
        [TestMethod]
        public void DefaultVariableIsDoubleAndIsZero()
        {
            Variable sut = new Variable("AA", new short[] { });
            Assert.AreEqual(0.0, sut.Value);
        }

        /// <summary>
        /// Test that variables marked as integers are a short with zero value.
        /// </summary>
        [TestMethod]
        public void IntegerVariableIsShortAndIsZero()
        {
            Variable sut = new Variable("AA%", new short[] { });
            Assert.AreEqual((short)0, sut.Value);
        }

        /// <summary>
        /// Test that variables marked as strings are a string with an empty value.
        /// </summary>
        [TestMethod]
        public void StringVariableIsEmptyString()
        {
            Variable sut = new Variable("AA$", new short[] { });
            Assert.AreEqual(string.Empty, sut.Value);
        }

        /// <summary>
        /// Double array can be created and is correct type and size
        /// </summary>
        [TestMethod]
        public void DefaultArrayVariableIsDoubleAndIsCorrectSizeAndName()
        {
            Variable sut = new Variable("AA", new short[] { 9, 4 });
            Assert.IsTrue(sut.Value is double[]);
            Assert.AreEqual("AA(", sut.Name);
            double[] array = (double[])sut.Value;
            Assert.AreEqual(50, array.Length);
        }

        /// <summary>
        /// Integer array can be created and is correct type and size
        /// </summary>
        [TestMethod]
        public void DefaultIntegerArrayVariableIsShortAndIsCorrectSizeAndName()
        {
            Variable sut = new Variable("AA%", new short[] { 9, 4 });
            Assert.IsTrue(sut.Value is short[]);
            Assert.AreEqual("AA%(", sut.Name);
            short[] array = (short[])sut.Value;
            Assert.AreEqual(50, array.Length);
        }

        /// <summary>
        /// String array can be created and is correct type and size
        /// </summary>
        [TestMethod]
        public void DefaultStringArrayVariableIsStringAndIsCorrectSizeAndName()
        {
            Variable sut = new Variable("AA$", new short[] { 9, 4 });
            Assert.IsTrue(sut.Value is string[]);
            Assert.AreEqual("AA$(", sut.Name);
            string[] array = (string[])sut.Value;
            Assert.AreEqual(50, array.Length);
        }

        /// <summary>
        /// Correct off set is generated when array is accessed.
        /// </summary>
        [TestMethod]
        public void CorrectOffsetIsGenerated()
        {
            Variable sut = new Variable("AA$", new short[] { 9, 4 });
            Assert.AreEqual(21, sut.Offset(new short[] { 1, 2 }));
        }

        /// <summary>
        /// Out of memory error when an array is too big. (64K elements)
        /// Out of memory error when an array is zero sized (actually -1 as 0 is valid).
        /// Out of memory error when an array is negative sized.
        /// </summary>
        /// <param name="dim1">First dimension.</param>
        /// <param name="dim2">Second dimension.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow(10, 10, false)]
        [DataRow(256, 356, true)]
        [DataRow(3, 0, false)]
        [DataRow(-1, 3, true)]
        [DataRow(-1, -3, true)]
        public void ArraySizesTests(int dim1, int dim2, bool throwsException)
        {
            Test.Throws<OutOfMemoryException>(
                () => new Variable("AA$", new short[] { (short)dim1, (short)dim2 }), throwsException);
        }

        /// <summary>
        /// Bad subscript error when there are too many dimensions.
        /// Bad subscript error when there are negative dimensions.
        /// Bad subscript error when a dimension is too big.
        /// </summary>
        /// <param name="dim1">First dimension.</param>
        /// <param name="dim2">Second dimension.</param>
        /// <param name="dim3">Optional dimension.</param>
        /// <param name="throwsException">Throws exception.</param>
        [DataTestMethod]
        [DataRow(7, 3, null, false)]
        [DataRow(11, 2, 3, true)]
        [DataRow(-2, 3, null, true)]
        [DataRow(10, 3, null, true)]
        [DataRow(3, 10, null, true)]
        public void BadSubscriptBasicExceptionTest(int dim1, int dim2, int? dim3, bool throwsException)
        {
            var indexes = new List<short>() { (short)dim1, (short)dim2 };
            if (dim3.HasValue)
            {
                indexes.Add((short)dim3.Value);
            }

            Variable sut = new Variable("AA$", new short[] { 9, 4 });
            Test.Throws<BadSubscriptException>(
                () => sut.Offset(indexes.ToArray()), throwsException);
        }
    }
}
