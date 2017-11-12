// <copyright file="Run.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RUN command.
    /// </summary>
    public class Run : Token, ITokeniserCommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;
        private readonly IDataStatementReader _dataStatementReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Run"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable Repository.</param>
        /// <param name="dataStatementReader">Data statement reader.</param>
        public Run(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository,
            IDataStatementReader dataStatementReader)
            : base("RUN", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
            _dataStatementReader = dataStatementReader;
        }

        /// <summary>
        /// Executes the RUN command.
        /// </summary>
        /// <param name="tokeniser">Tokeniser used by the load command.</param>
        public void Execute(ITokeniser tokeniser)
        {
            var nextToken = _runEnvironment.CurrentLine.NextToken();

            if (nextToken.TokenClass == TokenType.ClassString)
            {
                // Since we have a tokeniser, we can just fake being the executor/interpreter
                // and create our own LOAD command and call it.
                var oldLine = _runEnvironment.CurrentLine;
                _runEnvironment.CurrentLine = tokeniser.Tokenise($"LOAD {nextToken}");
                var loadToken = _runEnvironment.CurrentLine.NextToken() as ITokeniserCommand;
                loadToken.Execute(tokeniser);
                _runEnvironment.CurrentLine = oldLine;
            }
            else
            {
                _runEnvironment.CurrentLine.PushToken(nextToken);
            }

            _variableRepository.Clear();
            _runEnvironment.Clear();
            _dataStatementReader.RestoreToLineNumber(null);
            int? startingLineNumber = _expressionEvaluator.GetLineNumber();
            _runEnvironment.CurrentLine = startingLineNumber.HasValue ?
                _programRepository.GetLine(startingLineNumber.Value) :
                _programRepository.GetFirstLine();
        }
    }
}
