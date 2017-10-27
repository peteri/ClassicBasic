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
#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly
        // ProgramStack = new Stack<(bool isForLoop, ProgramLine programCounter)>();
#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
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

#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly

        // public Stack<(bool isForLoop, ProgramLine programCounter)> ProgramStack { get; }
#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
    }
}
