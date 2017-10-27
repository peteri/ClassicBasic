// <copyright file="IProgramRepository.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Repository for the program.
    /// </summary>
    public interface IProgramRepository
    {
        /// <summary>
        /// Gets a line by line number. Will throw if the line number cannot be found.
        /// </summary>
        /// <param name="lineNumber">Line number to get.</param>
        /// <returns>The program line.</returns>
        ProgramLine GetLine(int lineNumber);

        /// <summary>
        /// Gets the next line in the program given a line number.
        /// </summary>
        /// <param name="lineNumber">Line number to get.</param>
        /// <returns>The program line or null if no more lines.</returns>
        ProgramLine GetNextLine(int lineNumber);

        /// <summary>
        /// Gets the first line in the program or returns null if no program in memory.
        /// </summary>
        /// <returns>The program line.</returns>
        ProgramLine GetFirstLine();

        /// <summary>
        /// Adds or updates a program line, if an empty list of tokens is provided
        /// deletes the line.
        /// </summary>
        /// <param name="programLine">Program line to add, update or remove.</param>
        void SetProgramLine(ProgramLine programLine);

        /// <summary>
        /// Deletes line numbered between startLineNumber and endLineNumber inclusive.
        /// </summary>
        /// <param name="startLineNumber">Starting line to delete.</param>
        /// <param name="endLineNumber">Ending line to delete.</param>
        void DeleteProgramLines(int startLineNumber, int endLineNumber);

        /// <summary>
        /// Clears all the current program lines from memory.
        /// </summary>
        void Clear();
    }
}