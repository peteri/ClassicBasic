// <copyright file="Stop.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the STOP command
    /// </summary>
    public class Stop : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stop"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Stop(IRunEnvironment runEnvironment)
            : base("STOP", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Execute STOP, we do this by throwing an exception.
        /// </summary>
        public void Execute()
        {
            _runEnvironment.ContinueToken = _runEnvironment.CurrentLine.CurrentToken;
            _runEnvironment.OnErrorGotoLineNumber = null;
            throw new Exceptions.BreakException();
        }
    }
}
