// <copyright file="StringFunctions.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Test.FunctionTests
{
    using System.Collections.Generic;
    using System.Linq;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
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
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, true)]
        [DataRow(2, false)]
        [DataRow(3, false)]
        [DataRow(4, true)]
        public void StringTestMidDollarNeedsTwoParameters(int count, bool throwsException)
        {
            var sut = new MidDollar();

            var parameters = new List<Accumulator>
            {
                new Accumulator("ABCDEF"),
                new Accumulator(3.0),
                new Accumulator(2.0),
                new Accumulator(3.0)
            };

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters.Take(count).ToArray()),
                throwsException);
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
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, true)]
        [DataRow(2, false)]
        [DataRow(3, true)]
        public void StringTestLeftDollarNeedsTwoParameters(int count, bool throwsException)
        {
            var sut = new LeftDollar();

            var parameters = new List<Accumulator>
            {
                new Accumulator("ABCDEF"),
                new Accumulator(3.0),
                new Accumulator(2.0),
                new Accumulator(3.0)
            };

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters.Take(count).ToArray()),
                throwsException);
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
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, true)]
        [DataRow(2, false)]
        [DataRow(3, true)]
        public void StringTestRightDollarNeedsTwoParameters(int count, bool throwsException)
        {
            var sut = new RightDollar();

            var parameters = new List<Accumulator>
            {
                new Accumulator("ABCDEF"),
                new Accumulator(3.0),
                new Accumulator(2.0),
                new Accumulator(3.0)
            };

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters.Take(count).ToArray()),
                throwsException);
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
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void StringTestCharDollarNeedsOneParameter(int count, bool throwsException)
        {
            var sut = new CharDollar();

            var parameters = new List<Accumulator> { };
            for (int i = 0; i < count; i++)
            {
                parameters.Add(new Accumulator(3.0));
            }

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters),
                throwsException);
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
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void StringTestAscNeedsOneParameter(int count, bool throwsException)
        {
            var sut = new Asc();

            var parameters = new List<Accumulator> { };
            for (int i = 0; i < count; i++)
            {
                parameters.Add(new Accumulator("A"));
            }

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters),
                throwsException);
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
        /// Test LEN needs one parameters.
        /// </summary>
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void StringTestLenNeedsOneParameters(int count, bool throwsException)
        {
            var sut = new Len();

            var parameters = new List<Accumulator>
            {
                new Accumulator("ABCDEF"),
                new Accumulator(3.0),
            };

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters.Take(count).ToArray()),
                throwsException);
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
        /// Test STR$ needs one parameters.
        /// </summary>
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void StringTestStrDollarNeedsOneParameters(int count, bool throwsException)
        {
            var sut = new StrDollar();

            var parameters = new List<Accumulator>
            {
                new Accumulator(4.4),
                new Accumulator(3.0),
            };

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters.Take(count).ToArray()),
                throwsException);
        }

        /// <summary>
        /// Test STR$ with various parameters.
        /// </summary>
        /// <param name="parameter">Parameter to function.</param>
        /// <param name="expectedResult">Expected result.</param>
        /// <param name="throwsTypeMismatch">Throws type mismatch exception.</param>
        [DataTestMethod]
        [DataRow((short)-32, " -32", false)]
        [DataRow((short)0, " 0", false)]
        [DataRow((short)32, " 32", false)]
        [DataRow(-32.25, " -32.25", false)]
        [DataRow(0.0, " 0", false)]
        [DataRow(32.75, " 32.75", false)]
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
        /// Test VAL needs one parameters.
        /// </summary>
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void StringTestValNeedsOneParameters(int count, bool throwsException)
        {
            var sut = new Val();

            var parameters = new List<Accumulator>
            {
                new Accumulator("ABCDEF"),
                new Accumulator(3.0),
            };

            var result = Test.Throws<SyntaxErrorException, Accumulator>(
                () => sut.Execute(parameters.Take(count).ToArray()),
                throwsException);
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
