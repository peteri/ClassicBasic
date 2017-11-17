// <copyright file="GlassTeletype.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Console
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;
    using ClassicBasic.Interpreter;

    /// <summary>
    /// Glass teletype.
    /// </summary>
    public class GlassTeletype : ITeletype
    {
        private const int ReadAheadBuffer = 256;
        private string _initialCommand;
        private string _editText;

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
        /// Gets a value indicating whether the teletype supports editing.
        /// If the in or out is redirected then disallow edit command.
        /// </summary>
        public bool CanEdit => !(Console.IsInputRedirected || Console.IsOutputRedirected);

        /// <summary>
        /// Sets the edit text, when Read is called this text is displayed to
        /// the user who can then edit it.
        /// </summary>
        public string EditText
        {
            set
            {
                _editText = value;
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

            if (!string.IsNullOrEmpty(_editText))
            {
                var returnValue = EditLine(_editText);
                _editText = string.Empty;
                return returnValue;
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

        /// <summary>
        /// Read a character from the keyboard.
        /// </summary>
        /// <returns>Character user typed in.</returns>
        public char ReadChar()
        {
            var keyInfo = Console.ReadKey(true);
            return keyInfo.KeyChar;
        }

        private string EditLine(string editText)
        {
            Console.Write(editText);
            var handle = Native.GetStdHandle(Native.StdInputHandle);
            Native.CONSOLE_READCONSOLE_CONTROL control = default(Native.CONSOLE_READCONSOLE_CONTROL);

            control.Length = (uint)Marshal.SizeOf(control);
            control.InitialChars = (uint)editText.Length;
            control.ControlKeyState = 0;

            uint charsReadUnused = 0;
            int charactersToRead = editText.Length + ReadAheadBuffer;
            StringBuilder buffer = new StringBuilder(editText, charactersToRead);
            bool result = Native.ReadConsole(handle, buffer, (uint)charactersToRead, out charsReadUnused, ref control);
            if (result == false)
            {
                int err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }

            if (charsReadUnused > (uint)buffer.Length)
            {
                charsReadUnused = (uint)buffer.Length;
            }

            var text = buffer.ToString(0, (int)charsReadUnused);
            while ((charsReadUnused != 0) && !text.EndsWith(Environment.NewLine))
            {
                charactersToRead = ReadAheadBuffer;
                StringBuilder extraText = new StringBuilder(charactersToRead);
                control.InitialChars = 0;
                result = Native.ReadConsole(handle, buffer, (uint)charactersToRead, out charsReadUnused, ref control);
                if (result == false)
                {
                    int err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(err);
                }

                if (charsReadUnused > (uint)extraText.Length)
                {
                    charsReadUnused = (uint)extraText.Length;
                }

                text += extraText.ToString(0, (int)charsReadUnused);
            }

            if (text.EndsWith(Environment.NewLine))
            {
                return text.Substring(0, text.Length - Environment.NewLine.Length);
            }

            return text;
        }
    }
}
