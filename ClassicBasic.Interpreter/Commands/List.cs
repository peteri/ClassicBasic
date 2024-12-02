// <copyright file="List.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;

    /// <summary>
    /// Implements the LIST command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="List"/> class.
    /// </remarks>
    /// <param name="programRepository">Program Repository.</param>
    /// <param name="teletype">Output teletype to use.</param>
    /// <param name="runEnvironment">Run environment.</param>
    public class List(
        IProgramRepository programRepository,
        ITeletype teletype,
        IRunEnvironment runEnvironment) : Token("LIST", TokenClass.Statement), IInterruptableCommand
    {
        private ProgramLine? _currentLine;
        private int _startLine;
        private int _endLine;

        /// <summary>
        /// Called before execute, used to setup the line number range.
        /// </summary>
        public void Setup()
        {
            int? start = runEnvironment.CurrentLine.GetLineNumber();
            int? end = start;

            var token = runEnvironment.CurrentLine.NextToken();
            if (token.Seperator == TokenType.Minus || token.Seperator == TokenType.Comma)
            {
                end = runEnvironment.CurrentLine.GetLineNumber();
            }
            else
            {
                runEnvironment.CurrentLine.PushToken(token);
            }

            _startLine = start ?? 0;
            _endLine = end ?? ushort.MaxValue;

            _currentLine = programRepository.GetFirstLine();
            while (_currentLine != null && _currentLine.LineNumber < _startLine)
            {
                _currentLine = programRepository.GetNextLine(_currentLine.LineNumber.Value);
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

                teletype.Write(_currentLine.ToString());
                teletype.Write(Environment.NewLine);
                _currentLine = programRepository.GetNextLine(_currentLine.LineNumber.Value);
            }

            return _currentLine == null;
        }
    }
}
