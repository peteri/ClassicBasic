// <copyright file="Pop.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the POP command.
    /// </summary>
    public class Pop : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Pop(IRunEnvironment runEnvironment)
            : base("POP", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the POP command.
        /// </summary>
        public void Execute()
        {
        }
    }
}
