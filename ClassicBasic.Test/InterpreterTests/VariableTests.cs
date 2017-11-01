// <copyright file="VariableTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using ClassicBasic.Interpreter;
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
        /// </summary>
        [TestMethod]
        public void OutOfMemoryBasicExceptionWhenArrayIsGreaterThan64K()
        {
            {
                var exceptionThrown = false;
                try
                {
                    Variable sut = new Variable("AA$", new short[] { 256, 356 });
                }
                catch (ClassicBasic.Interpreter.Exceptions.OutOfMemoryException)
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        /// <summary>
        /// Out of memory error when an array is zero sized.
        /// </summary>
        [TestMethod]
        public void OutOfMemoryBasicExceptionWhenArrayIsZeroSize()
        {
            {
                var exceptionThrown = false;
                try
                {
                    // Note this is -1 as arrays start numbering from zero
                    // Dim(1,2) defines an array of (0..1,0..2) i.e. six elements.
                    Variable sut = new Variable("AA$", new short[] { 1, -1, 4 });
                }
                catch (ClassicBasic.Interpreter.Exceptions.OutOfMemoryException)
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        /// <summary>
        /// Out of memory error when an array is negative sized.
        /// </summary>
        [TestMethod]
        public void OutOfMemoryBasicExceptionWhenArrayIsNegative()
        {
            {
                var exceptionThrown = false;
                try
                {
                    Variable sut = new Variable("AA$", new short[] { -2, -3 });
                }
                catch (ClassicBasic.Interpreter.Exceptions.OutOfMemoryException)
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        /// <summary>
        /// Bad subscript error when there are too many dimensions.
        /// </summary>
        [TestMethod]
        public void BadSubscriptBasicExceptionWhenArrayHasWrongDimensions()
        {
            Variable sut = new Variable("AA$", new short[] { 9, 4 });
            var exceptionThrown = false;
            try
            {
                sut.Offset(new short[] { 11, 2, 3 });
            }
            catch (ClassicBasic.Interpreter.Exceptions.BadSubscriptException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Bad subscript error when there are negative dimensions.
        /// </summary>
        [TestMethod]
        public void BadSubscriptBasicExceptionWhenArrayHasNegativeDimensions()
        {
            Variable sut = new Variable("AA$", new short[] { 9, 4 });
            var exceptionThrown = false;
            try
            {
                sut.Offset(new short[] { -11, 2 });
            }
            catch (ClassicBasic.Interpreter.Exceptions.BadSubscriptException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Bad subscript error when a dimension is too big.
        /// </summary>
        [TestMethod]
        public void BadSubscriptBasicExceptionWhenDimensionsIsTooBig()
        {
            Variable sut = new Variable("AA$", new short[] { 9, 4 });
            var exceptionThrown = false;
            try
            {
                sut.Offset(new short[] { 9, 5 });
            }
            catch (ClassicBasic.Interpreter.Exceptions.BadSubscriptException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
