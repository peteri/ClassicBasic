// <copyright file="Save.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;

    /// <summary>
    /// Implements the Save command.
    /// </summary>
    public class Save : Token, IInterruptableCommand
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramRepository _programRepository;
        private readonly ITeletype _teletype;
        private ProgramLine _currentLine;
        private int _endLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Save"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="teletype">Output teletype to use.</param>
        public Save(
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository,
            ITeletype teletype)
            : base("SAVE", TokenType.ClassStatement)
        {
            _expressionEvaluator = expressionEvaluator;
            _programRepository = programRepository;
            _teletype = teletype;
        }

        /// <summary>
        /// Called before execute, used to setup the line number range.
        /// </summary>
        public void Setup()
        {
            _endLine = ushort.MaxValue;
            _currentLine = _programRepository.GetFirstLine();
        }

        /// <inheritdoc />
        public bool Execute()
        {
            if (_currentLine != null)
            {
                _teletype.Write($"{_currentLine.LineNumber} ");
                while (!_currentLine.EndOfLine)
                {
                    _teletype.Write(_currentLine.NextToken().ToString());
                }

                _teletype.Write(Environment.NewLine);
                _currentLine = _programRepository.GetNextLine(_currentLine.LineNumber.Value);
            }

            return _currentLine == null;
        }
    }
}
