// <copyright file="Stop.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the STOP command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Stop"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class Stop(IRunEnvironment runEnvironment) : Token("STOP", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Execute STOP, we do this by throwing an exception.
        /// </summary>
        public void Execute()
        {
            runEnvironment.ContinueToken = runEnvironment.CurrentLine.CurrentToken;
            runEnvironment.OnErrorGotoLineNumber = null;
            throw new Exceptions.BreakException();
        }
    }
}
