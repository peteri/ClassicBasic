// <copyright file="Get.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GET command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Get"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="teletypeWithPosition">Teletype.</param>
    public class Get(
       IRunEnvironment runEnvironment,
       IExpressionEvaluator expressionEvaluator,
       ITeletypeWithPosition teletypeWithPosition) : Token("GET", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the GET command.
        /// </summary>
        public void Execute()
        {
            if (!runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            var variableReference = expressionEvaluator.GetLeftValue();
            var newChar = teletypeWithPosition.ReadChar();
            Accumulator newValue;
            if (variableReference.IsString)
            {
                newValue = new Accumulator(newChar.ToString());
            }
            else
            {
                if ("+-E.\0".Contains(newChar.ToString()))
                {
                    newChar = '0';
                }

                if (newChar < '0' || newChar > '9')
                {
                    throw new Exceptions.SyntaxErrorException();
                }

                newValue = new Accumulator((double)(newChar - '0'));
            }

            variableReference.SetValue(newValue);
        }
    }
}
