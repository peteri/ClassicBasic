// <copyright file="Print.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the PRINT command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Print"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="teletype">Output teletype to use.</param>
    public class Print(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator,
        ITeletypeWithPosition teletype) : Token("PRINT", TokenClass.Statement, TokenType.Print), ICommand
    {
        /// <summary>
        /// Executes the PRINT command.
        /// </summary>
        public void Execute()
        {
            bool newLine = true;
            while (true)
            {
                var token = runEnvironment.CurrentLine.NextToken();
                if ((token.Seperator == TokenType.Colon)
                    || (token.Statement == TokenType.Else)
                    || (token.Seperator == TokenType.EndOfLine))
                {
                    if (newLine)
                    {
                        teletype.NewLine();
                    }

                    runEnvironment.CurrentLine.PushToken(token);
                    return;
                }

                if ((token.Statement == TokenType.Tab) || (token.Statement == TokenType.Space))
                {
                    short value = expressionEvaluator.GetExpression().ValueAsShort();
                    if (runEnvironment.CurrentLine.NextToken().Seperator != TokenType.CloseBracket)
                    {
                        throw new Exceptions.SyntaxErrorException();
                    }

                    if (token.Statement == TokenType.Tab)
                    {
                        teletype.Tab(value);
                    }
                    else
                    {
                        teletype.Space(value);
                    }
                }
                else if ((token.Seperator == TokenType.Semicolon) || (token.Seperator == TokenType.Comma))
                {
                    newLine = false;
                    if (token.Seperator == TokenType.Comma)
                    {
                        teletype.NextComma();
                    }
                }
                else
                {
                    newLine = true;
                    runEnvironment.CurrentLine.PushToken(token);
                    teletype.Write(expressionEvaluator.GetExpression().ToString());
                }
            }
        }
    }
}
