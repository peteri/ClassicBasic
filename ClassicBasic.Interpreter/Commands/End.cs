// <copyright file="End.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the END command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="End"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class End(IRunEnvironment runEnvironment) : Token("END", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Execute END, we do this by throwing an exception, this one doesn't
        /// display anything.
        /// </summary>
        public void Execute()
        {
            runEnvironment.ContinueToken = runEnvironment.CurrentLine.CurrentToken;
            runEnvironment.OnErrorGotoLineNumber = null;

            throw new Exceptions.EndException();
        }
    }
}
