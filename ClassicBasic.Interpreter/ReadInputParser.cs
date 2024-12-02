// <copyright file="ReadInputParser.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// ReadInputParser used by read and input to parse data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReadInputParser"/> class.
    /// </remarks>
    /// <param name="moreData">Function to call to get more data.</param>
    public class ReadInputParser(Func<string> moreData) : IReadInputParser
    {
        private string _currentLine = null;
        private int _currentIndex;

        /// <summary>
        /// Gets a value indicating whether there is extra data left.
        /// </summary>
        public bool HasExtraData => !string.IsNullOrEmpty(_currentLine);

        /// <summary>
        /// Clears the current line.
        /// </summary>
        public void Clear()
        {
            _currentLine = null;
        }

        /// <summary>
        /// Reads a list of variables from the current line.
        /// </summary>
        /// <param name="variableReferences">List of variable references to read.</param>
        public void ReadVariables(IEnumerable<VariableReference> variableReferences)
        {
            foreach (var variableReference in variableReferences)
            {
                var value = variableReference.IsString ? ReadString() : ReadNumber();
                variableReference.SetValue(value);
            }
        }

        private Accumulator ReadString()
        {
            return new Accumulator(GetNextToken());
        }

        private Accumulator ReadNumber()
        {
            var strNumber = GetNextToken();
            if (strNumber == string.Empty)
            {
                return new Accumulator(0.0);
            }

            if (double.TryParse(strNumber, out double returnValue))
            {
                return new Accumulator(returnValue);
            }

            throw new Exceptions.SyntaxErrorException();
        }

        private string GetNextToken()
        {
            bool quotedString = false;
            bool skipSpaces = false;

            StringBuilder returnValue = new();

            if (_currentLine == null)
            {
                _currentLine = moreData();
                _currentIndex = 0;
            }

            // Skip leading whitespace.
            while (_currentIndex < _currentLine.Length && char.IsWhiteSpace(_currentLine[_currentIndex]))
            {
                _currentIndex++;
            }

            while (_currentIndex < _currentLine.Length)
            {
                char c = _currentLine[_currentIndex++];
                if (c == '\"')
                {
                    // Quote at start of string into quotes mode.
                    if (returnValue.Length == 0)
                    {
                        quotedString = true;
                        continue;
                    }

                    // We were in quotes mode exit it and skip trailing spaces.
                    if (quotedString)
                    {
                        quotedString = false;
                        skipSpaces = true;
                        continue;
                    }
                }

                // Found final character.
                if (c == ',' && !quotedString)
                {
                    break;
                }

                // Skipping spaces? If not whitespace throw syntax error.
                if (skipSpaces)
                {
                    if (char.IsWhiteSpace(c))
                    {
                        continue;
                    }

                    throw new Exceptions.SyntaxErrorException();
                }

                // Safe to add to our output.
                returnValue.Append(c);
            }

            // We've run off the end
            if (_currentIndex >= _currentLine.Length)
            {
                _currentLine = null;
            }

            return returnValue.ToString();
        }
    }
}
