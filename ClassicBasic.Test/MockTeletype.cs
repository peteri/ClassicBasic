// <copyright file="MockTeletype.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ClassicBasic.Interpreter;

    /// <summary>
    /// Mock teletype for test usage
    /// </summary>
    public class MockTeletype : ITeletype
    {
        private Queue<string> _output;
        private Queue<string> _input;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockTeletype"/> class.
        /// </summary>
        public MockTeletype()
        {
            _output = new Queue<string>();
            _input = new Queue<string>();
        }

#pragma warning disable CS0067
        /// <inheritdoc/>
        public event ConsoleCancelEventHandler CancelEventHandler;
#pragma warning restore CS0067

        /// <summary>
        /// Gets Queue of strings used for input.
        /// </summary>
        public Queue<string> Input => _input;

        /// <summary>
        /// Gets List of output from interpreter.
        /// </summary>
        public Queue<string> Output => _output;

        /// <summary>
        /// Gets the width of the teletype.
        /// </summary>
        public short Width => 80;

        /// <summary>
        /// Gets or sets a value indicating whether the teletype supports editing
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the edit text, when Read is called this text is displayed to
        /// the user who can then edit it.
        /// </summary>
        public string EditText { get; set; }

        /// <inheritdoc/>
        public string Read()
        {
            if (_input.Count == 0)
            {
                return string.Empty;
            }

            var returnValue = _input.Dequeue();

            if (EditText != string.Empty)
            {
                returnValue = EditText + returnValue;
                EditText = string.Empty;
            }

            if (returnValue == "BREAK")
            {
                RaiseCancelEvent();
                return null;
            }

            return returnValue;
        }

        /// <inheritdoc/>
        public void Write(string output)
        {
            _output.Enqueue(output);
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

        /// <summary>
        /// Read a character from the keyboard.
        /// </summary>
        /// <returns>Character user typed in.</returns>
        public char ReadChar()
        {
            throw new NotImplementedException();
        }
    }
}
