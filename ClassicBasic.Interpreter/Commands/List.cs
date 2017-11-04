// <copyright file="List.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;

    /// <summary>
    /// Implements the LIST command.
    /// </summary>
    public class List : Token, IInterruptableCommand
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramRepository _programRepository;
        private readonly ITeletype _teletype;
        private readonly IRunEnvironment _runEnvironment;
        private ProgramLine _currentLine;
        private int _startLine;
        private int _endLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="List"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="teletype">Output teletype to use.</param>
        /// <param name="runEnvironment">Run environment.</param>
        public List(
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository,
            ITeletype teletype,
            IRunEnvironment runEnvironment)
            : base("LIST", TokenType.ClassStatement)
        {
            _expressionEvaluator = expressionEvaluator;
            _programRepository = programRepository;
            _teletype = teletype;
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Called before execute, used to setup the line number range.
        /// </summary>
        public void Setup()
        {
            int? start = _expressionEvaluator.GetLineNumber();
            int? end = start;

            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.Seperator == TokenType.Minus || token.Seperator == TokenType.Comma)
            {
                end = _expressionEvaluator.GetLineNumber();
            }
            else
            {
                _runEnvironment.CurrentLine.PushToken(token);
            }

            _startLine = start ?? 0;
            _endLine = end ?? ushort.MaxValue;

            _currentLine = _programRepository.GetFirstLine();
            while (_currentLine != null && _currentLine.LineNumber < _startLine)
            {
                _currentLine = _programRepository.GetNextLine(_currentLine.LineNumber.Value);
            }
        }

        /// <summary>
        /// Lists a single line.
        /// </summary>
        /// <returns>returns false if last line printed.</returns>
        public bool Execute()
        {
            if (_currentLine != null)
            {
                if (_currentLine.LineNumber > _endLine)
                {
                    return true;
                }

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
