// <copyright file="On.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the ON command.
    /// </summary>
    public class On : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="On"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program Repository.</param>
        public On(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository)
            : base("ON", TokenType.ClassStatement | TokenType.Gosub)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the ON command.
        /// </summary>
        public void Execute()
        {
            var counter = _expressionEvaluator.GetExpression().ValueAsShort();
            if ((counter < 0) || (counter > 255))
            {
                throw new Exceptions.IllegalQuantityException();
            }

            var type = _runEnvironment.CurrentLine.NextToken();
            if ((type.Statement != TokenType.Goto) && (type.Statement != TokenType.Gosub))
            {
                throw new Exceptions.SyntaxErrorException();
            }

            int? foundLine = null;
            IToken token;

            do
            {
                var lineNumber = _expressionEvaluator.GetLineNumber();
                if (!lineNumber.HasValue)
                {
                    throw new Exceptions.SyntaxErrorException();
                }

                counter--;
                if (counter == 0)
                {
                    foundLine = lineNumber;
                }

                token = _runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);

            // Put back next token.
            if (token.Seperator != TokenType.Comma)
            {
                _runEnvironment.CurrentLine.PushToken(token);
            }

            if (foundLine.HasValue)
            {
                if (type.Statement == TokenType.Gosub)
                {
                    var returnAddress = new StackEntry
                    {
                        Line = _runEnvironment.CurrentLine,
                        LineToken = _runEnvironment.CurrentLine.CurrentToken
                    };

                    _runEnvironment.ProgramStack.Push(returnAddress);
                    _runEnvironment.TestForStackOverflow();
                }

                _runEnvironment.CurrentLine = _programRepository.GetLine(foundLine.Value);
            }
        }
    }
}
