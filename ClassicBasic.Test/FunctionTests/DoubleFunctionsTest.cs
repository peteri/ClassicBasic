// <copyright file="DoubleFunctionsTest.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.FunctionTests
{
    using System;
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
    using ClassicBasic.Interpreter.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests all the functions that inherit from Double function
    /// </summary>
    [TestClass]
    public class DoubleFunctionsTest
    {
        /// <summary>
        /// Check that double functions requires at least one parameter (using abs).
        /// </summary>
        /// <param name="throwsException">Flag to say if exception should be thrown.</param>
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void DoubleFunctionsRequiresOneParameter(bool throwsException)
        {
            var sut = new Abs();
            var parameters = new List<Accumulator> { new Accumulator(3.0) };
            if (throwsException)
            {
                parameters.Add(new Accumulator(4.0));
            }

            Test.Throws<SyntaxErrorException, Accumulator>(
                   () => sut.Execute(parameters),
                   throwsException);
        }

        /// <summary>
        /// Check that double functions requires at least one parameter (using log).
        /// </summary>
        /// <param name="throwsException">Flag to say if exception should be thrown.</param>
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void PositiveFunctionsRequiresOneParameter(bool throwsException)
        {
            var sut = new Log();
            var parameters = new List<Accumulator> { new Accumulator(3.0) };
            if (throwsException)
            {
                parameters.Add(new Accumulator(4.0));
            }

            Test.Throws<SyntaxErrorException, Accumulator>(
                   () => sut.Execute(parameters),
                   throwsException);
        }

        /// <summary>
        /// Test Positive Abs.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionPositiveAbs()
        {
            var sut = new Abs();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(0.5) });
            Assert.AreEqual(0.5, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Negative Abs.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionNegativeAbs()
        {
            var sut = new Abs();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(-0.5) });
            Assert.AreEqual(0.5, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Atan.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionAtn()
        {
            var sut = new Atn();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(0.5) });
            Assert.AreEqual(Math.Atan(0.5), result.ValueAsDouble());
        }

        /// <summary>
        /// Test Cos.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionCos()
        {
            var sut = new Cos();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(Math.PI / 3) });
            Assert.AreEqual(Math.Cos(Math.PI / 3), result.ValueAsDouble());
        }

        /// <summary>
        /// Test Exp.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionExp()
        {
            var sut = new Exp();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(10.0) });
            Assert.AreEqual(Math.Exp(10.0), result.ValueAsDouble());
        }

        /// <summary>
        /// Test Int of 10.3.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionInt10point3()
        {
            var sut = new Int();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(10.3) });
            Assert.AreEqual(10.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Int of 10.7.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionInt10point7()
        {
            var sut = new Int();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(10.7) });
            Assert.AreEqual(10.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Int of 10.3.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionIntNegative10point3()
        {
            var sut = new Int();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(-10.3) });
            Assert.AreEqual(-11.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Int of 10.7.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionNegativeInt10point7()
        {
            var sut = new Int();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(-10.7) });
            Assert.AreEqual(-11.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Log.
        /// </summary>
        /// <param name="value">value to test with.</param>
        /// <param name="throwsException">Does the value throw an exception.</param>
        [DataTestMethod]
        [DataRow(10.0, false)]
        [DataRow(0.0, true)]
        [DataRow(-10.0, true)]
        public void DoubleFunctionLog(double value, bool throwsException)
        {
            var sut = new Log();
            var result = Test.Throws<IllegalQuantityException, Accumulator>(
                () => sut.Execute(new List<Accumulator> { new Accumulator(value) }),
                throwsException);
            if (!throwsException)
            {
                Assert.AreEqual(Math.Log(value), result.ValueAsDouble());
            }
        }

        /// <summary>
        /// Test Sgn.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionSgn()
        {
            var sut = new Sgn();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(10.0) });
            Assert.AreEqual(1.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test negative Sgn.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionNegativeSgn()
        {
            var sut = new Sgn();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(-10.0) });
            Assert.AreEqual(-1.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Sgn.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionZeroSgn()
        {
            var sut = new Sgn();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(0.0) });
            Assert.AreEqual(0.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Test Sin.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionSin()
        {
            var sut = new Sin();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(Math.PI / 3) });
            Assert.AreEqual(Math.Sin(Math.PI / 3), result.ValueAsDouble());
        }

        /// <summary>
        /// Test Square root.
        /// </summary>
        /// <param name="value">value to test with.</param>
        /// <param name="throwsException">Does the value throw an exception.</param>
        [DataTestMethod]
        [DataRow(10.0, false)]
        [DataRow(0.0, false)]
        [DataRow(-10.0, true)]
        public void DoubleFunctionSqr(double value, bool throwsException)
        {
            var sut = new Sqr();
            var result = Test.Throws<IllegalQuantityException, Accumulator>(
                () => sut.Execute(new List<Accumulator> { new Accumulator(value) }),
                throwsException);
            if (!throwsException)
            {
                Assert.AreEqual(Math.Sqrt(value), result.ValueAsDouble());
            }
        }

        /// <summary>
        /// Test Tan.
        /// </summary>
        [TestMethod]
        public void DoubleFunctionTan()
        {
            var sut = new Tan();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(Math.PI / 3) });
            Assert.AreEqual(Math.Tan(Math.PI / 3), result.ValueAsDouble());
        }
    }
}
