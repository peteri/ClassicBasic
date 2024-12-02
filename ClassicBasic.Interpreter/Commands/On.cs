// <copyright file="On.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ON command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="On"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="programRepository">Program Repository.</param>
    public class On(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator,
        IProgramRepository programRepository) : Token("ON", TokenClass.Statement, TokenType.Gosub), ICommand
    {
        /// <summary>
        /// Executes the ON command.
        /// </summary>
        public void Execute()
        {
            var counter = expressionEvaluator.GetExpression().ValueAsShort();
            if ((counter < 0) || (counter > 255))
            {
                throw new Exceptions.IllegalQuantityException();
            }

            var type = runEnvironment.CurrentLine.NextToken();
            if ((type.Statement != TokenType.Goto) && (type.Statement != TokenType.Gosub))
            {
                throw new Exceptions.SyntaxErrorException();
            }

            int? foundLine = null;
            IToken token;

            do
            {
                var lineNumber = runEnvironment.CurrentLine.GetLineNumber();
                if (!lineNumber.HasValue)
                {
                    throw new Exceptions.SyntaxErrorException();
                }

                counter--;
                if (counter == 0)
                {
                    foundLine = lineNumber;
                }

                token = runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);

            // Put back next token.
            runEnvironment.CurrentLine.PushToken(token);

            if (foundLine.HasValue)
            {
                if (type.Statement == TokenType.Gosub)
                {
                    var returnAddress = new StackEntry
                    {
                        Line = runEnvironment.CurrentLine,
                        LineToken = runEnvironment.CurrentLine.CurrentToken,
                    };

                    runEnvironment.ProgramStack.Push(returnAddress);
                    runEnvironment.TestForStackOverflow();
                }

                runEnvironment.CurrentLine = programRepository.GetLine(foundLine.Value);
            }
        }
    }
}
