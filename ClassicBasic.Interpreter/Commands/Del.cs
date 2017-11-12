﻿// <copyright file="Del.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>
namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the DEL command.
    /// </summary>
    public class Del : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Del"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run environment.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        public Del(
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExpressionEvaluator expressionEvaluator)
            : base("DEL", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <summary>
        /// Executes the DEL command.
        /// </summary>
        public void Execute()
        {
            int? start = _expressionEvaluator.GetLineNumber();
            int? end = null;
            if (start.HasValue)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                if (token.Seperator == TokenType.Comma)
                {
                    end = _expressionEvaluator.GetLineNumber();
                }
            }

            if (!start.HasValue || !end.HasValue)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            _programRepository.DeleteProgramLines(start.Value, end.Value);

            // If we're in a program end us and don't allow continue.
            if (_runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                _runEnvironment.ContinueLineNumber = null;
                throw new Exceptions.EndException();
            }
        }
    }
}
