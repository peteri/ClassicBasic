// <copyright file="Print.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the PRINT command.
    /// </summary>
    public class Print : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly ITeletypeWithPosition _teletype;

        /// <summary>
        /// Initializes a new instance of the <see cref="Print"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="teletype">Output teletype to use.</param>
        public Print(IRunEnvironment runEnvironment, IExpressionEvaluator expressionEvaluator, ITeletypeWithPosition teletype)
            : base("PRINT", TokenType.ClassStatement | TokenType.Print)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _teletype = teletype;
        }

        /// <summary>
        /// Executes the PRINT command
        /// </summary>
        public void Execute()
        {
            bool newLine = true;
            while (true)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                if ((token.Seperator == TokenType.Colon)
                    || (token.Statement == TokenType.Else)
                    || (token.Seperator == TokenType.EndOfLine))
                {
                    if (newLine)
                    {
                        _teletype.NewLine();
                    }

                    _runEnvironment.CurrentLine.PushToken(token);
                    return;
                }

                if ((token.Statement == TokenType.Tab) || (token.Statement == TokenType.Space))
                {
                    short value = _expressionEvaluator.GetExpression().ValueAsShort();
                    if (_runEnvironment.CurrentLine.NextToken().Seperator != TokenType.CloseBracket)
                    {
                        throw new Exceptions.SyntaxErrorException();
                    }

                    if (token.Statement == TokenType.Tab)
                    {
                        _teletype.Tab(value);
                    }
                    else
                    {
                        _teletype.Space(value);
                    }
                }
                else if ((token.Seperator == TokenType.Semicolon) || (token.Seperator == TokenType.Comma))
                {
                    newLine = false;
                    if (token.Seperator == TokenType.Comma)
                    {
                        _teletype.NextComma();
                    }
                }
                else
                {
                    newLine = true;
                    _runEnvironment.CurrentLine.PushToken(token);
                    _teletype.Write(_expressionEvaluator.GetExpression().ToString());
                }
            }
        }
    }
}
