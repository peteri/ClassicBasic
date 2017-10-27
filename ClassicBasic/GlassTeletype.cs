// <copyright file="GlassTeletype.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Console
{
    using System;
    using ClassicBasic.Interpreter;

    /// <summary>
    /// Glass teletype.
    /// </summary>
    public class GlassTeletype : ITeletype
    {
        private string _initialCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassTeletype"/> class.
        /// </summary>
        /// <param name="initialCommand">Initial command for the interpreter.</param>
        public GlassTeletype(string initialCommand)
        {
            _initialCommand = initialCommand;
        }

        /// <summary>
        /// Event handler for cancel (aka Ctrl-C).
        /// </summary>
        event ConsoleCancelEventHandler ITeletype.CancelEventHandler
        {
            add
            {
                System.Console.CancelKeyPress += value;
            }

            remove
            {
                System.Console.CancelKeyPress -= value;
            }
        }

        /// <summary>
        /// Read a line from the console or if an initial command has been specified the intial command.
        /// </summary>
        /// <returns>Line of text from the console.</returns>
        public string Read()
        {
            if (!string.IsNullOrEmpty(_initialCommand))
            {
                var tempinitialCommand = _initialCommand;
                _initialCommand = string.Empty;
                return tempinitialCommand;
            }

            return System.Console.ReadLine();
        }

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="output">string to output.</param>
        public void Write(string output)
        {
            System.Console.Write(output);
        }
    }
}
