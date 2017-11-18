// <copyright file="Else.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ELSE command.
    /// </summary>
    public class Else : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Else"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Else(IRunEnvironment runEnvironment)
            : base("ELSE", TokenClass.Statement, TokenType.Else)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the ELSE command, this only gets executed when an IF statement is true
        /// and skips until we hit end of line.
        /// </summary>
        public void Execute()
        {
            while (!_runEnvironment.CurrentLine.EndOfLine)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
            }
        }
    }
}
