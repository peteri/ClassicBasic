// <copyright file="OnErr.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ONERR command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OnErr"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class OnErr(IRunEnvironment runEnvironment) : Token("ONERR", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the ONERR command.
        /// </summary>
        public void Execute()
        {
            var token = runEnvironment.CurrentLine.NextToken();
            var lineNumber = runEnvironment.CurrentLine.GetLineNumber();
            if (token.Statement != TokenType.Goto || !lineNumber.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            runEnvironment.OnErrorGotoLineNumber = (lineNumber.Value == 0) ? (int?)null : lineNumber.Value;
        }
    }
}
