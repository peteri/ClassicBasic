// <copyright file="RunEnvironment.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;

    /// <summary>
    /// Grab bag of stuff for the run time environment.
    /// </summary>
    public class RunEnvironment : IRunEnvironment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunEnvironment"/> class.
        /// </summary>
        public RunEnvironment()
        {
            ProgramStack = new Stack<StackEntry>();
            DefinedFunctions = new Dictionary<string, UserDefinedFunction>();
        }

        /// <summary>
        /// Gets or sets the current line being executed.
        /// </summary>
        public ProgramLine CurrentLine { get; set; }

        /// <summary>
        /// Gets or sets the line the CONT command will restart with.
        /// </summary>
        public int? ContinueLineNumber { get; set; }

        /// <summary>
        /// Gets or sets the token within the line the CONT command will restart with.
        /// </summary>
        public int ContinueToken { get; set; }

        /// <summary>
        /// Gets or sets the override for an error on a data line.
        /// </summary>
        public int? DataErrorLine { get; set; }

        /// <summary>
        /// Gets or sets the last error line used by the resume statement
        /// </summary>
        public int? LastErrorLine { get; set; }

        /// <summary>
        /// Gets or sets the token for a resume
        /// </summary>
        public int LastErrorToken { get; set; }

        /// <summary>
        /// Gets or sets the last error number.
        /// </summary>
        public int LastErrorNumber { get; set; }

        /// <summary>
        /// Gets or sets the last error stack count.
        /// </summary>
        public int LastErrorStackCount { get; set; }

        /// <summary>
        /// Gets or sets the line number to goto for on error.
        /// </summary>
        public int? OnErrorGotoLineNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user hit the break key.
        /// </summary>
        public bool KeyboardBreak { get; set; }

        /// <summary>
        /// Gets stack for GOSUB/RETURN or FOR/NEXT
        /// </summary>
        public Stack<StackEntry> ProgramStack { get; }

        /// <summary>
        /// Gets the user defined functions dictionary.
        /// </summary>
        public Dictionary<string, UserDefinedFunction> DefinedFunctions { get; }

        /// <summary>
        /// Clears down the run environment.
        /// </summary>
        public void Clear()
        {
            ProgramStack.Clear();
            DefinedFunctions.Clear();
            ContinueLineNumber = null;
            OnErrorGotoLineNumber = null;
            LastErrorNumber = 0;
            LastErrorLine = null;
        }

        /// <summary>
        /// Handles on error calls.
        /// </summary>
        /// <param name="programRepository">Program repository to use</param>
        /// <param name="errorCode">Error code of the exception.</param>
        public void OnErrorHandler(IProgramRepository programRepository, int errorCode)
        {
            LastErrorLine = ContinueLineNumber;
            LastErrorToken = ContinueToken;
            LastErrorNumber = errorCode;
            LastErrorStackCount = ProgramStack.Count;

            // This might throw an undefined statement error and loop
            // endlessly but then it does that on an Apple ][
            CurrentLine = programRepository.GetLine(OnErrorGotoLineNumber.Value);
        }

        /// <summary>
        /// Tests if the program stack has more than 50 entries, if so throws out of memory exception.
        /// </summary>
        public void TestForStackOverflow()
        {
            if (ProgramStack.Count > 50)
            {
                throw new Exceptions.OutOfMemoryException();
            }
        }
    }
}
