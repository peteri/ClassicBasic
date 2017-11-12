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
        /// Gets or sets the override for an error on a data line.
        /// </summary>
        int? DataErrorLine { get; set; }

        /// <summary>
        /// Gets or sets the last error line used by the resume statement
        /// </summary>
        int? LastErrorLine { get; set; }

        /// <summary>
        /// Gets or sets the token for a resume
        /// </summary>
        int LastErrorToken { get; set; }

        /// <summary>
        /// Gets or sets the last error number.
        /// </summary>
        int LastErrorNumber { get; set; }

        /// <summary>
        /// Gets or sets the last error stack count.
        /// </summary>
        int LastErrorStackCount { get; set; }

        /// <summary>
        /// Gets or sets the line number to goto for on error.
        /// </summary>
        int? OnErrorGotoLineNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user hit the break key.
        /// </summary>
        bool KeyboardBreak { get; set; }

        /// <summary>
        /// Gets stack for GOSUB/RETURN or FOR/NEXT
        /// </summary>
        Stack<StackEntry> ProgramStack { get; }

        /// <summary>
        /// Gets the user defined functions dictionary.
        /// </summary>
        Dictionary<string, UserDefinedFunction> DefinedFunctions { get; }

        /// <summary>
        /// Clears down the run environment.
        /// </summary>
        void Clear();

        /// <summary>
        /// Handles on error calls.
        /// </summary>
        /// <param name="programRepository">Program repository to use</param>
        void OnErrorHandler(IProgramRepository programRepository);

        /// <summary>
        /// Tests if the program stack has more than 50 entries, if so throws out of memory exception
        /// </summary>
        void TestForStackOverflow();
    }
}
