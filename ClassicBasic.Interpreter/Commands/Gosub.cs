// <copyright file="Gosub.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the GOSUB command.
    /// </summary>
    public class Gosub : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gosub"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public Gosub(IRunEnvironment runEnvironment, IExpressionEvaluator expressionEvaluator)
            : base("GOSUB", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the GOSUB command.
        /// </summary>
        public void Execute()
        {
        }
    }
}
