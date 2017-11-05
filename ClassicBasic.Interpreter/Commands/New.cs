// <copyright file="New.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the new command.
    /// </summary>
    public class New : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="New"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable Repository.</param>
        public New(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository)
            : base("NEW", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
        }

        /// <summary>
        /// Executes the NEW command.
        /// </summary>
        public void Execute()
        {
            _variableRepository.Clear();
            _runEnvironment.ProgramStack.Clear();
            _runEnvironment.DefinedFunctions.Clear();
            _runEnvironment.ContinueLineNumber = null;
            _programRepository.Clear();
        }
    }
}
