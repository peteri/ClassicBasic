// <copyright file="Del.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DEL command.
    /// </summary>
    public class Del : Token, ICommand
    {
        private readonly IProgramRepository _programRepository;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Del"/> class.
        /// </summary>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public Del(IProgramRepository programRepository, IExpressionEvaluator expressionEvaluator)
            : base("DEL", TokenType.ClassStatement)
        {
            _programRepository = programRepository;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the DEL command.
        /// </summary>
        public void Execute()
        {
        }
    }
}
