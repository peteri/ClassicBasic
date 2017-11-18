// <copyright file="Native.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Wrapper class for native Windows calls
    /// </summary>
    internal static class Native
    {
        /// <summary>
        /// Standard input handle.
        /// </summary>
        public const int StdInputHandle = -10;

        /// <summary>
        /// Get the standard handle.
        /// </summary>
        /// <param name="nStdHandle">Stand handle.</param>
        /// <returns>Handle.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);  // param is NOT a handle, but it returns one!

        /// <summary>
        /// Reads a data from the console, allows tab completion (or in our case
        /// stuffing in text for edting.
        /// </summary>
        /// <param name="consoleInput">Handle for the console.</param>
        /// <param name="buffer">buffer for the text</param>
        /// <param name="numberOfCharsToRead">number of characters to read.</param>
        /// <param name="numberOfCharsRead">Number of characters read.</param>
        /// <param name="controlData">Control data structure.</param>
        /// <returns>error code.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool ReadConsole(
            IntPtr consoleInput,
            StringBuilder buffer,
            uint numberOfCharsToRead,
            out uint numberOfCharsRead,
            ref CONSOLE_READCONSOLE_CONTROL controlData);

        /// <summary>
        /// Struct used to pass information to ReadConsole.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_READCONSOLE_CONTROL
        {
            /// <summary>Length of buffer.</summary>
            internal uint Length;

            /// <summary>Number of initial characters to skip.</summary>
            internal uint InitialChars;

            /// <summary>32 bit mask for ctrl character wakeup i.e. bit 8 set means Tab.</summary>
            internal uint CtrlWakeupMask;

            /// <summary>
            /// State of control keys when a ctrl wake mask key is hit, used to
            /// do tab completion, so we know if we should Tab or Shift-Tab.
            /// </summary>
            internal /* out */ uint ControlKeyState;
        }
    }
}
