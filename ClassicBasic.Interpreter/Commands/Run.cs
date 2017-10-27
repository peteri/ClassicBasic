// <copyright file="Run.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RUN command.
    /// </summary>
    public class Run : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Run"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable Repository.</param>
        public Run(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository)
            : base("RUN", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
        }

        /// <summary>
        /// Executes the RUN command.
        /// </summary>
        public void Execute()
        {
            var nextToken = _runEnvironment.CurrentLine.NextToken();

            if (nextToken.TokenClass == TokenType.ClassString)
            {
#warning fix me
                // This will load the program eventually.
                throw new Exceptions.BasicException("Not implemented yet");
            }
            else
            {
                _runEnvironment.CurrentLine.PushToken(nextToken);
            }

            _variableRepository.Clear();
            int? startingLineNumber = _expressionEvaluator.GetLineNumber();
            _runEnvironment.CurrentLine = startingLineNumber.HasValue ?
                _programRepository.GetLine(startingLineNumber.Value) :
                _programRepository.GetFirstLine();
        }
    }
}
