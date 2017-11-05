// <copyright file="Get.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GET command.
    /// </summary>
    public class Get : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly ITeletypeWithPosition _teletypeWithPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Get"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="teletypeWithPosition">Teletype.</param>
        public Get(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            ITeletypeWithPosition teletypeWithPosition)
            : base("GET", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _teletypeWithPosition = teletypeWithPosition;
        }

        /// <summary>
        /// Executes the GET command.
        /// </summary>
        public void Execute()
        {
            if (!_runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            var variableReference = _expressionEvaluator.GetLeftValue();
            var newChar = _teletypeWithPosition.ReadChar();
            Accumulator newValue = new Accumulator(newChar.ToString());
            variableReference.SetValue(newValue);
        }
    }
}
