// <copyright file="ProgramRepository.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Repository of program lines.
    /// </summary>
    public class ProgramRepository : IProgramRepository
    {
        private readonly SortedList<int, ProgramLine> _program;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramRepository"/> class.
        /// </summary>
        public ProgramRepository()
        {
            _program = new SortedList<int, ProgramLine>();
        }

        /// <summary>
        /// Gets the first line in the program or returns null if no program in memory.
        /// </summary>
        /// <returns>The program line.</returns>
        public ProgramLine GetFirstLine()
        {
            return (_program.Count == 0) ? null : GetLine(_program.Values[0].LineNumber.Value);
        }

        /// <summary>
        /// Gets a line by line number. Will throw if the line number cannot be found.
        /// </summary>
        /// <param name="lineNumber">Line number to get.</param>
        /// <returns>The program line.</returns>
        public ProgramLine GetLine(int lineNumber)
        {
            if (_program.ContainsKey(lineNumber))
            {
                _program[lineNumber].CurrentToken = 0;
                return _program[lineNumber];
            }

            throw new Exceptions.UndefinedStatementException();
        }

        /// <summary>
        /// Gets the next line in the program given a line number.
        /// </summary>
        /// <param name="lineNumber">Line number to get.</param>
        /// <returns>The program line or null if no more lines.</returns>
        public ProgramLine GetNextLine(int lineNumber)
        {
            var currentLine = _program.IndexOfKey(lineNumber);
            currentLine++;
            if (currentLine >= _program.Count)
            {
                // Ran off the end of the program
                return null;
            }

            return GetLine(_program.Keys[currentLine]);
        }

        /// <summary>
        /// Adds or updates a program line, if an empty list of tokens is provided
        /// deletes the line.
        /// </summary>
        /// <param name="programLine">Program line to add, update or remove.</param>
        public void SetProgramLine(ProgramLine programLine)
        {
            programLine.CurrentToken = 0;
            if (programLine.EndOfLine)
            {
                _program.Remove(programLine.LineNumber.Value);
            }
            else
            {
                _program[programLine.LineNumber.Value] = programLine;
            }
        }

        /// <summary>
        /// Deletes line numbered between startLineNumber and endLineNumber inclusive.
        /// </summary>
        /// <param name="startLineNumber">Starting line to delete.</param>
        /// <param name="endLineNumber">Ending line to delete.</param>
        public void DeleteProgramLines(int startLineNumber, int endLineNumber)
        {
            var linesToDelete = _program.Values.Where(pl => pl.LineNumber >= startLineNumber && pl.LineNumber <= endLineNumber)
                .Select(pl => pl.LineNumber.Value)
                .ToArray();
            foreach (var line in linesToDelete)
            {
                _program.Remove(line);
            }
        }

        /// <summary>
        /// Clears all the current program lines from memory.
        /// </summary>
        public void Clear()
        {
            _program.Clear();
        }
    }
}
