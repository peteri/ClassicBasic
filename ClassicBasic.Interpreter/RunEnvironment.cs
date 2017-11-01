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
        /// Gets or sets a value indicating whether user hit the break key.
        /// </summary>
        public bool KeyboardBreak { get; set; }

        /// <summary>
        /// Gets stack for GOSUB/RETURN or FOR/NEXT
        /// </summary>
        public Stack<StackEntry> ProgramStack { get; }

        /// <summary>
        /// Tests if the program stack has more than 50 entries, if so throws out of memory exception
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
