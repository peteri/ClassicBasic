// <copyright file="Del.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DEL command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Del"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run environment.</param>
    /// <param name="programRepository">Program Repository.</param>
    public class Del(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository) : Token("DEL", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the DEL command.
        /// </summary>
        public void Execute()
        {
            int? start = runEnvironment.CurrentLine.GetLineNumber();
            int? end = null;
            if (start.HasValue)
            {
                var token = runEnvironment.CurrentLine.NextToken();
                if (token.Seperator == TokenType.Comma)
                {
                    end = runEnvironment.CurrentLine.GetLineNumber();
                }
            }

            if (!start.HasValue || !end.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            programRepository.DeleteProgramLines(start.Value, end.Value);

            // If we're in a program end us and don't allow continue.
            if (runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                runEnvironment.ContinueLineNumber = null;
                throw new Exceptions.EndException();
            }
        }
    }
}
