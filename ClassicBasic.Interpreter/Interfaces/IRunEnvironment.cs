// <copyright file="IRunEnvironment.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;

    /// <summary>
    /// Run time environment.
    /// </summary>
    public interface IRunEnvironment
    {
        /// <summary>
        /// Gets or sets the current line being executed.
        /// </summary>
        ProgramLine CurrentLine { get; set; }

        /// <summary>
        /// Gets or sets the line the CONT command will restart with.
        /// </summary>
        int? ContinueLineNumber { get; set; }

        /// <summary>
        /// Gets or sets the token within the line the CONT command will restart with.
        /// </summary>
        int ContinueToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user hit the break key.
        /// </summary>
        bool KeyboardBreak { get; set; }

        /// <summary>
        /// Gets stack for GOSUB/RETURN or FOR/NEXT
        /// </summary>
        Stack<StackEntry> ProgramStack { get; }

        /// <summary>
        /// Tests if the program stack has more than 50 entries, if so throws out of memory exception
        /// </summary>
        void TestForStackOverflow();
    }
}
