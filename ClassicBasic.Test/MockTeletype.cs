// <copyright file="MockTeletype.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using System;
    using System.Reflection;
    using ClassicBasic.Interpreter;

    /// <summary>
    /// Mock teletype for test usage
    /// </summary>
    public class MockTeletype : ITeletype
    {
#pragma warning disable CS0067
        /// <inheritdoc/>
        public event ConsoleCancelEventHandler CancelEventHandler;
#pragma warning restore CS0067

        /// <inheritdoc/>
        public string Read()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Write(string output)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Raises a cancel event in the real system.
        /// </summary>
        /// <returns>Instance of the ConsoleCancelEventArgs used.</returns>
        public ConsoleCancelEventArgs RaiseCancelEvent()
        {
            var types = new Type[] { typeof(ConsoleSpecialKey) };
            var ctor = typeof(ConsoleCancelEventArgs).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                types,
                null);
            var cancelEventArgs = (ConsoleCancelEventArgs)ctor.Invoke(new object[] { ConsoleSpecialKey.ControlBreak });
            CancelEventHandler.Invoke(this, cancelEventArgs);
            return cancelEventArgs;
        }
    }
}
