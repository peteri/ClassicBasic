// <copyright file="MiscFunctions.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.FunctionTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
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
        [TestMethod]
        public void FunctionFreRequiresOneParameter()
        {
            var exceptionThrown = false;
            var sut = new Fre();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator(3.0), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
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
        [TestMethod]
        public void FunctionPosRequiresOneParameter()
        {
            var exceptionThrown = false;
            var mockTeletypeWithPosition = new Mock<ITeletypeWithPosition>();
            var sut = new Pos(mockTeletypeWithPosition.Object);

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator(3.0), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
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
        [TestMethod]
        public void FunctionRndRequiresOneParameter()
        {
            var exceptionThrown = false;
            var sut = new Rnd();

            try
            {
                sut.Execute(new List<Accumulator> { new Accumulator(3.0), new Accumulator(3.0) });
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
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
