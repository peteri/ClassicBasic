// <copyright file="MiscFunctions.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.FunctionTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
    using ClassicBasic.Interpreter.Functions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Test class for various functions that aren't double or string based.
    /// </summary>
    [TestClass]
    public class MiscFunctions
    {
        /// <summary>
        /// Check that fre requires at least one parameter.
        /// </summary>
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void FunctionFreRequiresOneParameter(int count, bool throwsException)
        {
            var sut = new Fre();
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
        /// Check that FRE returns 48000.
        /// </summary>
        [TestMethod]
        public void FunctionFreReturnsExpectedValue()
        {
            var sut = new Fre();
            var result = sut.Execute(new List<Accumulator> { new Accumulator(3.0) });
            Assert.AreEqual(48000, result.ValueAsDouble());
        }

        /// <summary>
        /// Check that Pos requires at least one parameter.
        /// </summary>
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void FunctionPosRequiresOneParameter(int count, bool throwsException)
        {
            var mockTeletypeWithPosition = new Mock<ITeletypeWithPosition>();
            var sut = new Pos(mockTeletypeWithPosition.Object);
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
        /// Check that POS returns the teletype position.
        /// </summary>
        [TestMethod]
        public void FunctionPosReturnsExpectedValue()
        {
            var mockTeletypeWithPosition = new Mock<ITeletypeWithPosition>();
            var sut = new Pos(mockTeletypeWithPosition.Object);

            mockTeletypeWithPosition.Setup(mtwp => mtwp.Position()).Returns((short)9);

            var result = sut.Execute(new List<Accumulator> { new Accumulator(3.0) });

            Assert.AreEqual(9, result.ValueAsDouble());
        }

        /// <summary>
        /// Check that Rnd requires at least one parameter.
        /// </summary>
        /// <param name="count">Count of parameters to pass</param>
        /// <param name="throwsException">True if exception thrown.</param>
        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        public void FunctionRndRequiresOneParameter(int count, bool throwsException)
        {
            var sut = new Rnd();
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
        /// Check that RND returns random numbers and can be given a seed.
        /// </summary>
        [TestMethod]
        public void FunctionRndReturnsRandomNumbers()
        {
            var sut = new Rnd();
            var result1 = sut.Execute(new List<Accumulator> { new Accumulator(-31.0) });
            var result2 = sut.Execute(new List<Accumulator> { new Accumulator(1.0) });
            var result3 = sut.Execute(new List<Accumulator> { new Accumulator(1.0) });

            Assert.AreNotEqual(result1.ValueAsDouble(), result2.ValueAsDouble());
            Assert.AreNotEqual(result1.ValueAsDouble(), result3.ValueAsDouble());
            Assert.AreNotEqual(result2.ValueAsDouble(), result3.ValueAsDouble());
        }

        /// <summary>
        /// Check that RND returns random numbers and can be given a seed.
        /// </summary>
        [TestMethod]
        public void FunctionRndReturnsRandomAndCanBeRepeated()
        {
            var sut = new Rnd();
            var result1 = sut.Execute(new List<Accumulator> { new Accumulator(-1024.0) });
            var result2 = sut.Execute(new List<Accumulator> { new Accumulator(1.0) });
            var result2repeated = sut.Execute(new List<Accumulator> { new Accumulator(0.0) });
            var result3 = sut.Execute(new List<Accumulator> { new Accumulator(1.0) });

            Assert.AreNotEqual(result1.ValueAsDouble(), result2.ValueAsDouble());
            Assert.AreNotEqual(result1.ValueAsDouble(), result3.ValueAsDouble());
            Assert.AreNotEqual(result2.ValueAsDouble(), result3.ValueAsDouble());
            Assert.AreEqual(result2.ValueAsDouble(), result2repeated.ValueAsDouble());
        }
    }
}
