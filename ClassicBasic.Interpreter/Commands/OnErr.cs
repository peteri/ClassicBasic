// <copyright file="OnErr.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ONERR command.
    /// </summary>
    public class OnErr : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnErr"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public OnErr(IRunEnvironment runEnvironment)
            : base("ONERR", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the ONERR command.
        /// </summary>
        public void Execute()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            var lineNumber = _runEnvironment.CurrentLine.GetLineNumber();
            if (token.Statement != TokenType.Goto || !lineNumber.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            _runEnvironment.OnErrorGotoLineNumber = (lineNumber.Value == 0) ? (int?)null : lineNumber.Value;
        }
    }
}
