// <copyright file="StringFunctions.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Test.FunctionTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for the string functions.
    /// </summary>
    [TestClass]
    public class StringFunctions
    {
        /// <summary>
        /// Test MID$ needs two or three parameters.
        /// </summary>
        [TestMethod]
        public void StringTestMidDollarNeedsTwoOrThreeParameter()
        {
            var exceptionThrown = false;
            var sut = new MidDollar();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator("A") });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test MID$ with various parameters.
        /// </summary>
        /// <param name="parameter1">First parameter.</param>
        /// <param name="parameter2">Second parameter.</param>
        /// <param name="parameter3">Third parameter (or null if missing).</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsIllegalQuantity">Throws illegal quantity exception.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow("", 1.0, 1.0, "", false, false)]
        [DataRow("ABCDEF", 1.0, 3.0, "ABC", false, false)]
        [DataRow("ABCDEF", 2.0, 3.0, "BCD", false, false)]
        [DataRow("ABCDEF", 6.0, 1.0, "F", false, false)]
        [DataRow("ABCDEF", 7.0, 1.0, "", false, false)]
        [DataRow("ABCDEF", 2.0, null, "BCDEF", false, false)]
        [DataRow("ABCDEF", 6.0, null, "F", false, false)]
        [DataRow("ABCDEF", 7.0, null, "", false, false)]
        [DataRow("ABCDEF", 0.0, 1.0, "", true, false)]
        [DataRow("ABCDEF", 255.0, 1.0, "", false, false)]
        [DataRow("ABCDEF", 256.0, 1.0, "", true, false)]
        [DataRow("ABCDEF", 2.0, -1.0, "", true, false)]
        [DataRow("ABCDEF", 2.0, 0.0, "", false, false)]
        [DataRow("ABCDEF", 2.0, 255.0, "BCDEF", false, false)]
        [DataRow("ABCDEF", 2.0, 256.0, "", true, false)]
        [DataRow(1.0, 0.0, 2.2, "", false, true)]
        [DataRow("", "", 2.2, "", false, true)]
        [DataRow("", 0.0, "", "", false, true)]
        public void StringTestMidDollar(
            object parameter1,
            object parameter2,
            object parameter3,
            string expectedResult,
            bool throwsIllegalQuantity,
            bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var illegalQuantExceptionThrown = false;
            var sut = new MidDollar();
            string result;

            try
            {
                var parameters = new List<Accumulator> { new Accumulator(parameter1), new Accumulator(parameter2) };
                if (parameter3 != null)
                {
                    parameters.Add(new Accumulator(parameter3));
                }

                result = sut.Execute(parameters).ValueAsString();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                illegalQuantExceptionThrown = true;
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsIllegalQuantity, illegalQuantExceptionThrown, "Illegal quant");
            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test LEFT$ needs two parameters.
        /// </summary>
        [TestMethod]
        public void StringTestLeftDollarNeedsTwoParameters()
        {
            var exceptionThrown = false;
            var sut = new LeftDollar();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test LEFT$ with various parameters.
        /// </summary>
        /// <param name="parameter1">First parameter.</param>
        /// <param name="parameter2">Second parameter.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsIllegalQuantity">Throws illegal quantity exception.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow("", 1.0, "", false, false)]
        [DataRow("ABCDEF", 1.0, "A", false, false)]
        [DataRow("ABCDEF", 2.0, "AB", false, false)]
        [DataRow("ABCDEF", 6.0, "ABCDEF", false, false)]
        [DataRow("ABCDEF", 7.0, "ABCDEF", false, false)]
        [DataRow("ABCDEF", 0.0, "ABCDEF", true, false)]
        [DataRow("ABCDEF", 255.0, "ABCDEF", false, false)]
        [DataRow("ABCDEF", 256.0, "ABCDEF", true, false)]
        [DataRow(0.0, 2.2, "ABCDEF", false, true)]
        [DataRow("A", "ABCDEF", "ABCDEF", false, true)]
        public void StringTestLeftDollar(object parameter1, object parameter2, string expectedResult, bool throwsIllegalQuantity, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var illegalQuantExceptionThrown = false;
            var sut = new LeftDollar();
            string result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter1), new Accumulator(parameter2) })
                    .ValueAsString();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                illegalQuantExceptionThrown = true;
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsIllegalQuantity, illegalQuantExceptionThrown, "Illegal quant");
            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test RIGHT$ needs two parameters.
        /// </summary>
        [TestMethod]
        public void StringTestRightDollarNeedsTwoParameters()
        {
            var exceptionThrown = false;
            var sut = new RightDollar();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test RIGHT$ with various parameters.
        /// </summary>
        /// <param name="parameter1">First parameter.</param>
        /// <param name="parameter2">Second parameter.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsIllegalQuantity">Throws illegal quantity exception.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow("", 1.0, "", false, false)]
        [DataRow("ABCDEF", 1.0, "F", false, false)]
        [DataRow("ABCDEF", 2.0, "EF", false, false)]
        [DataRow("ABCDEF", 6.0, "ABCDEF", false, false)]
        [DataRow("ABCDEF", 7.0, "ABCDEF", false, false)]
        [DataRow("ABCDEF", 0.0, "ABCDEF", true, false)]
        [DataRow("ABCDEF", 255.0, "ABCDEF", false, false)]
        [DataRow("ABCDEF", 256.0, "ABCDEF", true, false)]
        [DataRow(0.0, 2.2, "ABCDEF", false, true)]
        [DataRow("A", "ABCDEF", "ABCDEF", false, true)]
        public void StringTestRightDollar(object parameter1, object parameter2, string expectedResult, bool throwsIllegalQuantity, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var illegalQuantExceptionThrown = false;
            var sut = new RightDollar();
            string result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter1), new Accumulator(parameter2) })
                    .ValueAsString();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                illegalQuantExceptionThrown = true;
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsIllegalQuantity, illegalQuantExceptionThrown, "Illegal quant");
            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test Char dollar needs one parameter.
        /// </summary>
        [TestMethod]
        public void StringTestCharDollarNeedsOneParameter()
        {
            var exceptionThrown = false;
            var sut = new CharDollar();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator("A"), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test CHR$ with various parameters.
        /// </summary>
        /// <param name="parameter">Parameter to function.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsIllegalQuantity">Throws illegal quantity exception.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow(32.0, " ", false, false)]
        [DataRow(65.0, "A", false, false)]
        [DataRow(short.MaxValue, "翿", false, false)]
        [DataRow(short.MinValue, "耀", false, false)]
        [DataRow(-1.0 + short.MinValue, "", true, false)]
        [DataRow(1.0 + short.MaxValue, "", true, false)]
        [DataRow("ABCDEF", "", false, true)]
        public void StringTestCharDollar(object parameter, string expectedResult, bool throwsIllegalQuantity, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var illegalQuantExceptionThrown = false;
            var sut = new CharDollar();
            string result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter) }).ValueAsString();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                illegalQuantExceptionThrown = true;
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsIllegalQuantity, illegalQuantExceptionThrown, "Illegal quant");
            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test ASC needs one parameter.
        /// </summary>
        [TestMethod]
        public void StringTestAscNeedsOneParameter()
        {
            var exceptionThrown = false;
            var sut = new Asc();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator("A"), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test ASC with various parameters.
        /// </summary>
        /// <param name="parameter">Parameter to function.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsIllegalQuantity">Throws illegal quantity exception.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow(" ", (short)32, false, false)]
        [DataRow("012345678", (short)48, false, false)]
        [DataRow("A", (short)65, false, false)]
        [DataRow("翿", short.MaxValue, false, false)]
        [DataRow("耀", short.MinValue, false, false)]
        [DataRow("", (short)0, true, false)]
        [DataRow(54.0, (short)0, false, true)]
        public void StringTestAsc(object parameter, short expectedResult, bool throwsIllegalQuantity, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var illegalQuantExceptionThrown = false;
            var sut = new Asc();
            short result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter) }).ValueAsShort();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalQuantityException)
            {
                illegalQuantExceptionThrown = true;
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsIllegalQuantity, illegalQuantExceptionThrown, "Illegal quant");
            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test LEN needs one parameter.
        /// </summary>
        [TestMethod]
        public void StringTestLenNeedsOneParameter()
        {
            var exceptionThrown = false;
            var sut = new Len();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator("A"), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test LEN with various parameters.
        /// </summary>
        /// <param name="parameter">Parameter to function.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow("", (short)0, false)]
        [DataRow("012345678", (short)9, false)]
        [DataRow("A", (short)1, false)]
        [DataRow(54.0, (short)0, true)]
        public void StringTestLen(object parameter, short expectedResult, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var sut = new Len();
            short result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter) }).ValueAsShort();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test Str$ needs one parameter.
        /// </summary>
        [TestMethod]
        public void StringTestStrDollarNeedsOneParameter()
        {
            var exceptionThrown = false;
            var sut = new StrDollar();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator("A"), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test STR$ with various parameters.
        /// </summary>
        /// <param name="parameter">Parameter to function.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow((short)-32, "-32", false)]
        [DataRow((short)0, "0", false)]
        [DataRow((short)32, "32", false)]
        [DataRow(-32.25, "-32.25", false)]
        [DataRow(0.0, "0", false)]
        [DataRow(32.75, "32.75", false)]
        [DataRow("A", "XX", true)]
        public void StringTestStrDollar(object parameter, string expectedResult, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var sut = new StrDollar();
            string result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter) }).ValueAsString();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }

        /// <summary>
        /// Test VAL needs one parameter.
        /// </summary>
        [TestMethod]
        public void StringTestValNeedsOneParameter()
        {
            var exceptionThrown = false;
            var sut = new Val();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator("A"), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test VAL with various parameters.
        /// </summary>
        /// <param name="parameter">Parameter to function.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow("", 0.0, false)]
        [DataRow("A", 0.0, false)]
        [DataRow("012345678", 12345678.0, false)]
        [DataRow("12.5", 12.5, false)]
        [DataRow("12.5E12", 1.25e13, false)]
        [DataRow(54.0, 0.0, true)]
        public void StringTestVal(object parameter, double expectedResult, bool throwsTypeMismatch)
        {
            var mismatchTypeExceptionThrown = false;
            var sut = new Val();
            double result;

            try
            {
                result = sut.Execute(new List<Accumulator> { new Accumulator(parameter) }).ValueAsDouble();
                Assert.AreEqual(expectedResult, result);
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                mismatchTypeExceptionThrown = true;
            }

            Assert.AreEqual(throwsTypeMismatch, mismatchTypeExceptionThrown, "Mismatch");
        }
    }
}
