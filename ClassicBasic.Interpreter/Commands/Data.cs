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
            : base("DATA", TokenType.ClassStatement | TokenType.Data)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the DATA command, skips until we hit end of line or colon.
        /// </summary>
        public void Execute()
        {
            while (!_runEnvironment.CurrentLine.EndOfLine)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                if (token.Statement == TokenType.Colon)
                {
                    _runEnvironment.CurrentLine.PushToken(token);
                    return;
                }
            }
        }
    }
}
