// <copyright file="Remark.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the REM command.
    /// </summary>
    public class Remark : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Remark"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Remark(IRunEnvironment runEnvironment)
            : base("REM", TokenType.ClassStatement | TokenType.Remark)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the REM command.
        /// </summary>
        public void Execute()
        {
            _runEnvironment.CurrentLine.NextToken();
        }
    }
}
