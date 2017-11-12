// <copyright file="End.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the END command.
    /// </summary>
    public class End : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="End"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public End(IRunEnvironment runEnvironment)
            : base("END", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Execute END, we do this by throwing an exception, this one doesn't
        /// display anything.
        /// </summary>
        public void Execute()
        {
            _runEnvironment.ContinueToken = _runEnvironment.CurrentLine.CurrentToken;
            _runEnvironment.OnErrorGotoLineNumber = null;

            throw new Exceptions.EndException();
        }
    }
}
