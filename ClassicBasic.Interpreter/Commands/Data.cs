// <copyright file="Data.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DATA command.
    /// </summary>
    public class Data : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Data(IRunEnvironment runEnvironment)
            : base("DATA", TokenClass.Statement, TokenType.Data)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the DATA command, skips the next token if it's class is ClassData.
        /// </summary>
        public void Execute()
        {
            if (!_runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            var token = _runEnvironment.CurrentLine.NextToken();

            if (token.TokenClass != TokenClass.Data)
            {
                throw new Exceptions.SyntaxErrorException();
            }
        }
    }
}
