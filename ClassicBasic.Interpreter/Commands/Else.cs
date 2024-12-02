// <copyright file="Else.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ELSE command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Else"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class Else(IRunEnvironment runEnvironment) : Token("ELSE", TokenClass.Statement, TokenType.Else), ICommand
    {
        /// <summary>
        /// Executes the ELSE command, this only gets executed when an IF statement is true
        /// and skips until we hit end of line.
        /// </summary>
        public void Execute()
        {
            while (!runEnvironment.CurrentLine.EndOfLine)
            {
                runEnvironment.CurrentLine.NextToken();
            }
        }
    }
}
