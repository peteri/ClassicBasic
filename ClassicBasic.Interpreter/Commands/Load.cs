// <copyright file="Load.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the LOAD command.
    /// </summary>
    public class Load : Token, ITokeniserCommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Load"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public Load(IRunEnvironment runEnvironment, IExpressionEvaluator expressionEvaluator)
            : base("LOAD", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the LOAD comand.
        /// </summary>
        /// <param name="tokeniser">Tokeniser to use.</param>
        public void Execute(ITokeniser tokeniser)
        {
        }
    }
}
