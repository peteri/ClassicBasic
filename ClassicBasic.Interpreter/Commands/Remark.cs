// <copyright file="Remark.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the REM command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Remark"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class Remark(IRunEnvironment runEnvironment) : Token("REM", TokenClass.Statement, TokenType.Remark), ICommand
    {
        /// <summary>
        /// Executes the REM command.
        /// </summary>
        public void Execute()
        {
            runEnvironment.CurrentLine.NextToken();
        }
    }
}
