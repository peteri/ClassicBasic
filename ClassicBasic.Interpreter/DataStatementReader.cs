// <copyright file="DataStatementReader.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Class that handles getting the next data line.
    /// </summary>
    public class DataStatementReader : IDataStatementReader
    {
        private readonly IProgramRepository _programRepository;
        private ProgramLine _currentDataLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStatementReader"/> class.
        /// </summary>
        /// <param name="programRepository">Program repository to use.</param>
        public DataStatementReader(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
            ReadInputParser = new ReadInputParser(GetNextDataStatement);
        }

        /// <summary>
        /// Gets the shared read input parser.
        /// </summary>
        public ReadInputParser ReadInputParser { get; }

        /// <summary>
        /// Implements moving the current data pointer to a new line number
        /// </summary>
        /// <param name="lineNumber">line number to move to, null moves to beginning of program.</param>
        public void RestoreToLineNumber(int? lineNumber)
        {
            _currentDataLine = lineNumber.HasValue
                ? _programRepository.GetLine(lineNumber.Value)
                : _programRepository.GetFirstLine();
            ReadInputParser.Clear();
        }

        private string GetNextDataStatement()
        {
            if (_currentDataLine == null)
            {
                throw new Exceptions.OutOfDataException();
            }

            while (_currentDataLine != null)
            {
                while (!_currentDataLine.EndOfLine)
                {
                    var token = _currentDataLine.NextToken();
                    if (token.TokenClass == TokenType.ClassData)
                    {
                        return token.Text;
                    }
                }

                _currentDataLine = _programRepository.GetNextLine(_currentDataLine.LineNumber.Value);
            }

            throw new Exceptions.OutOfDataException();
        }
    }
}
