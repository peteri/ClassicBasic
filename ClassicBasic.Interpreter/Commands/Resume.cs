// <copyright file="Resume.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RESUME command.
    /// </summary>
    public class Resume : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resume"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Resume(IRunEnvironment runEnvironment)
            : base("RESUME", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the RESUME command.
        /// </summary>
        public void Execute()
        {
        }
    }
}
