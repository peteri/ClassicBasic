// <copyright file="Gosub.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GOSUB command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Gosub"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="programRepository">Program Repository.</param>
    public class Gosub(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository) : Token("GOSUB", TokenClass.Statement, TokenType.Gosub), ICommand
    {
        /// <summary>
        /// Executes the GOSUB command.
        /// </summary>
        public void Execute()
        {
            var nextLine = runEnvironment.CurrentLine.GetLineNumber();
            if (!nextLine.HasValue)
            {
                throw new Exceptions.UndefinedStatementException();
            }

            var returnAddress = new StackEntry
            {
                Line = runEnvironment.CurrentLine,
                LineToken = runEnvironment.CurrentLine.CurrentToken,
            };

            runEnvironment.CurrentLine = programRepository.GetLine(nextLine.Value);
            runEnvironment.ProgramStack.Push(returnAddress);
            runEnvironment.TestForStackOverflow();
        }
    }
}
