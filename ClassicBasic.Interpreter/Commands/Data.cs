// <copyright file="Data.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DATA command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Data"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class Data(IRunEnvironment runEnvironment) : Token("DATA", TokenClass.Statement, TokenType.Data), ICommand
    {
        /// <summary>
        /// Executes the DATA command, skips the next token if it's class is ClassData.
        /// </summary>
        public void Execute()
        {
            if (!runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            var token = runEnvironment.CurrentLine.NextToken();

            if (token.TokenClass != TokenClass.Data)
            {
                throw new Exceptions.SyntaxErrorException();
            }
        }
    }
}
