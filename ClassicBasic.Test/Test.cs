// <copyright file="Test.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Helper test class.
    /// </summary>
    public static class Test
    {
        /// <summary>
        /// Tests if an action throws an exception
        /// </summary>
        /// <typeparam name="T">Type of the exception.</typeparam>
        /// <param name="action">Action to test.</param>
        /// <returns>Exception thrown.</returns>
        public static T Throws<T>(Action action)
            where T : Exception
        {
            return Throws<T>(action, true);
        }

        /// <summary>
        /// Tests if an action throws an exception
        /// </summary>
        /// <typeparam name="T">Type of the exception.</typeparam>
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="function">Action to test.</param>
        public static void Throws<T, R>(Func<R> function)
            where T : Exception
        {
            Throws<T, R>(function, true);
        }

        /// <summary>
        /// Tests if an action throws an exception
        /// </summary>
        /// <typeparam name="T">Type of the exception.</typeparam>
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="function">Action to test.</param>
        /// <param name="throwsException">Flag to say if exception should be thrown.</param>
        /// <returns>Value</returns>
        public static R Throws<T, R>(Func<R> function, bool throwsException)
            where T : Exception
        {
            bool exceptionThrown = false;
            R returnValue = default(R);
            try
            {
                returnValue = function();
            }
            catch (T)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown);
            return returnValue;
        }

        /// <summary>
        /// Tests if an action throws an exception
        /// </summary>
        /// <typeparam name="T">Type of the exception.</typeparam>
        /// <param name="action">Action to test.</param>
        /// <param name="throwsException">Flag to say if exception should be thrown.</param>
        /// <returns>Exception thrown.</returns>
        public static T Throws<T>(Action action, bool throwsException)
            where T : Exception
        {
            bool exceptionThrown = false;
            T returnValue = null;
            try
            {
                action();
            }
            catch (T exception)
            {
                returnValue = exception;
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown);
            return returnValue;
        }
    }
}
