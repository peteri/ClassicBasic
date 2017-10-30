// <copyright file="StringFunctions.cs" company="Peter Ibbotson">
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
    /// Test class for the string functions.
    /// </summary>
    [TestClass]
    public class StringFunctions
    {
        /// <summary>
        /// Test Left dollar needs two parameters.
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
    }
}
