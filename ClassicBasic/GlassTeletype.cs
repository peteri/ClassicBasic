// <copyright file="GlassTeletype.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Console
{
    using System;
    using System.Text;
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
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
        }

        /// <summary>
        /// Event handler for cancel (aka Ctrl-C).
        /// </summary>
        event ConsoleCancelEventHandler ITeletype.CancelEventHandler
        {
            add
            {
                Console.CancelKeyPress += value;
            }

            remove
            {
                Console.CancelKeyPress -= value;
            }
        }

        /// <summary>
        /// Gets the width of the teletype.
        /// </summary>
        public short Width => (short)Console.WindowWidth;

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

            return Console.ReadLine();
        }

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="output">string to output.</param>
        public void Write(string output)
        {
            Console.Write(output);
        }
    }
}
