// <copyright file="VariableRepositoryTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Variable Repository class.
    /// </summary>
    [TestClass]
    public class VariableRepositoryTests
    {
        /// <summary>
        /// Double test for initial value.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryReturnsZeroDoubleOnFirstUse()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("N", new short[] { });
            var accumulator = variableReference.GetValue();
            Assert.AreEqual(0.0, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(double), accumulator.Type);
        }

        /// <summary>
        /// Short test for initial value.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryReturnsZeroShortOnFirstUse()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("N%", new short[] { });
            var accumulator = variableReference.GetValue();
            Assert.AreEqual((short)0, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(short), accumulator.Type);
        }

        /// <summary>
        /// String test for initial value.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryReturnsEmptyStringOnFirstUse()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("N$", new short[] { });
            var accumulator = variableReference.GetValue();
            Assert.AreEqual(string.Empty, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(string), accumulator.Type);
        }

        /// <summary>
        /// Array names are truncated to two characters.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTruncatesNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("ABCD", new short[] { });
            variableReference.SetValue(new Accumulator(4.0));
            var variableReference2 = sut.GetOrCreateVariable("ABEF", new short[] { });
            var accumulator = variableReference.GetValue();
            Assert.AreEqual(4.0, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(double), accumulator.Type);
        }

        /// <summary>
        /// Array names are truncated to two characters.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTruncatesArrayNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("ABCD", new short[] { 2, 3 });
            var variableReference2 = sut.GetOrCreateVariable("ABEF", new short[] { 2, 3 });

            variableReference.SetValue(new Accumulator(4.0));
            var accumulator = variableReference2.GetValue();
            Assert.AreEqual(4.0, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(double), accumulator.Type);
        }

        /// <summary>
        /// Different array names in second character.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryDistinguishesArrayNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("AB", new short[] { 2, 3 });
            var variableReference2 = sut.GetOrCreateVariable("AC", new short[] { 2, 3 });

            variableReference.SetValue(new Accumulator(4.0));
            var accumulator = variableReference2.GetValue();
            Assert.AreEqual(0.0, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(double), accumulator.Type);
        }

        /// <summary>
        /// Short array names are truncated to two characters.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTruncatesShortNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("ABCD%", new short[] { });
            variableReference.SetValue(new Accumulator(4.0));
            var variableReference2 = sut.GetOrCreateVariable("ABEF%", new short[] { });
            var accumulator = variableReference.GetValue();
            Assert.AreEqual((short)4, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(short), accumulator.Type);
        }

        /// <summary>
        /// Short array names are truncated to two characters.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTruncatesShortArrayNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("ABCD%", new short[] { 2, 3 });
            var variableReference2 = sut.GetOrCreateVariable("ABEF%", new short[] { 2, 3 });

            variableReference.SetValue(new Accumulator((short)4));
            var accumulator = variableReference2.GetValue();
            Assert.AreEqual((short)4, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(short), accumulator.Type);
        }

        /// <summary>
        /// Different short array names in second character.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryDistinguishesShortArrayNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("AB%", new short[] { 2, 3 });
            var variableReference2 = sut.GetOrCreateVariable("AC%", new short[] { 2, 3 });

            variableReference.SetValue(new Accumulator(4.0));
            var accumulator = variableReference2.GetValue();
            Assert.AreEqual((short)0, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(short), accumulator.Type);
        }

        /// <summary>
        /// String array names are truncated to two characters.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTruncatesStringNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("ABCD$", new short[] { });
            variableReference.SetValue(new Accumulator("HELLO"));
            var variableReference2 = sut.GetOrCreateVariable("ABEF$", new short[] { });
            var accumulator = variableReference.GetValue();
            Assert.AreEqual("HELLO", accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(string), accumulator.Type);
        }

        /// <summary>
        /// String array names are truncated to two characters.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTruncatesStringArrayNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("ABCD$", new short[] { 2, 3 });
            var variableReference2 = sut.GetOrCreateVariable("ABEF$", new short[] { 2, 3 });

            variableReference.SetValue(new Accumulator("HELLO"));
            var accumulator = variableReference2.GetValue();
            Assert.AreEqual("HELLO", accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(string), accumulator.Type);
        }

        /// <summary>
        /// Different string array names in second character.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryDistinguishesStringArrayNamesToTwoCharacters()
        {
            IVariableRepository sut = new VariableRepository();
            var variableReference = sut.GetOrCreateVariable("AB$", new short[] { 2, 3 });
            var variableReference2 = sut.GetOrCreateVariable("AC$", new short[] { 2, 3 });

            variableReference.SetValue(new Accumulator("HELLO"));
            var accumulator = variableReference2.GetValue();
            Assert.AreEqual(string.Empty, accumulator.GetValue(accumulator.Type));
            Assert.AreEqual(typeof(string), accumulator.Type);
        }

        /// <summary>
        /// Test types and arrays occupy different name spaces.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTypesAndArraysOccupyDifferentSpaces()
        {
            IVariableRepository sut = new VariableRepository();
            var shortReference = sut.GetOrCreateVariable("AB%", new short[] { });
            var doubleReference = sut.GetOrCreateVariable("AB", new short[] { });
            var stringReference = sut.GetOrCreateVariable("AB$", new short[] { });
            var shortArrayReference = sut.GetOrCreateVariable("AB%", new short[] { 2, 3 });
            var doubleArrayReference = sut.GetOrCreateVariable("AB", new short[] { 2, 3 });
            var stringArrayReference = sut.GetOrCreateVariable("AB$", new short[] { 2, 3 });

            shortReference.SetValue(new Accumulator((short)1));
            shortArrayReference.SetValue(new Accumulator((short)2));
            stringReference.SetValue(new Accumulator("3"));
            stringArrayReference.SetValue(new Accumulator("4"));
            doubleReference.SetValue(new Accumulator(5.0));
            doubleArrayReference.SetValue(new Accumulator(6.0));

            var shortReference2 = sut.GetOrCreateVariable("AB%", new short[] { });
            var doubleReference2 = sut.GetOrCreateVariable("AB", new short[] { });
            var stringReference2 = sut.GetOrCreateVariable("AB$", new short[] { });
            var shortArrayReference2 = sut.GetOrCreateVariable("AB%", new short[] { 2, 3 });
            var doubleArrayReference2 = sut.GetOrCreateVariable("AB", new short[] { 2, 3 });
            var stringArrayReference2 = sut.GetOrCreateVariable("AB$", new short[] { 2, 3 });

            Assert.AreEqual((short)1, shortReference2.GetValue().GetValue(typeof(short)));
            Assert.AreEqual((short)2, shortArrayReference2.GetValue().GetValue(typeof(short)));
            Assert.AreEqual("3", stringReference2.GetValue().GetValue(typeof(string)));
            Assert.AreEqual("4", stringArrayReference2.GetValue().GetValue(typeof(string)));
            Assert.AreEqual(5.0, doubleReference2.GetValue().GetValue(typeof(double)));
            Assert.AreEqual(6.0, doubleArrayReference2.GetValue().GetValue(typeof(double)));
        }

        /// <summary>
        /// Test Dimension is correct size
        /// </summary>
        /// <param name="first">First array index</param>
        /// <param name="second">Second array index</param>
        /// <param name="shouldThrow">Should throw</param>
        [DataTestMethod]
        [DataRow((short)2, (short)3, false)]
        [DataRow((short)3, (short)3, true)]
        [DataRow((short)2, (short)4, true)]
        public void VariableRepositoryTestDimension(short first, short second, bool shouldThrow)
        {
            IVariableRepository sut = new VariableRepository();
            bool exceptionThrown = false;
            sut.DimensionArray("AB", new short[] { 2, 3 });
            var doubleArrayReference = sut.GetOrCreateVariable("AB", new short[] { first, second });
            try
            {
                doubleArrayReference.SetValue(new Accumulator(2.0));
            }
            catch (ClassicBasic.Interpreter.Exceptions.BadSubscriptException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(shouldThrow, exceptionThrown);
        }

        /// <summary>
        /// Test array redimension throws exception.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTestRedimensionThrowsException()
        {
            IVariableRepository sut = new VariableRepository();
            bool exceptionThrown = false;
            sut.DimensionArray("AB", new short[] { 2, 3 });
            try
            {
                sut.DimensionArray("AB", new short[] { 2, 3 });
            }
            catch (ClassicBasic.Interpreter.Exceptions.RedimensionedArrayException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        /// <summary>
        /// Test array sizes default to 10
        /// </summary>
        /// <param name="first">First array index</param>
        /// <param name="second">Second array index</param>
        /// <param name="shouldThrow">Should throw</param>
        [DataTestMethod]
        [DataRow((short)10, (short)10, false)]
        [DataRow((short)11, (short)10, true)]
        [DataRow((short)10, (short)11, true)]
        public void VariableRepositoryTestDefaultSize(short first, short second, bool shouldThrow)
        {
            IVariableRepository sut = new VariableRepository();
            bool exceptionThrown = false;
            var doubleArrayReference = sut.GetOrCreateVariable("AB", new short[] { 2, 3 });
            var doubleArrayReference2 = sut.GetOrCreateVariable("AB", new short[] { first, second });
            try
            {
                doubleArrayReference2.SetValue(new Accumulator(2.0));
            }
            catch (ClassicBasic.Interpreter.Exceptions.BadSubscriptException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(shouldThrow, exceptionThrown);
        }

        /// <summary>
        /// Test Clear.
        /// </summary>
        [TestMethod]
        public void VariableRepositoryTestClear()
        {
            IVariableRepository sut = new VariableRepository();
            var doubleReference = sut.GetOrCreateVariable("AB", new short[] { 0, 0 });
            doubleReference.SetValue(new Accumulator(4.0));
            sut.Clear();
            var doubleReference2 = sut.GetOrCreateVariable("AB", new short[] { 0, 0 });
            Assert.AreEqual(0.0, doubleReference2.GetValue().ValueAsDouble());
        }
    }
}
