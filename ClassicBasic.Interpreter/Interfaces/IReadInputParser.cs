// <copyright file="IReadInputParser.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;

    /// <summary>
    /// IReadInputParser used by read and input to parse data.
    /// </summary>
    public interface IReadInputParser
    {
        /// <summary>
        /// Gets a value indicating whether there is extra data left.
        /// </summary>
        bool HasExtraData { get; }

        /// <summary>
        /// Clears the current line.
        /// </summary>
        void Clear();

        /// <summary>
        /// Reads a list of variables from the current line.
        /// </summary>
        /// <param name="variableReferences">List of variable references to read.</param>
        void ReadVariables(IEnumerable<VariableReference> variableReferences);
    }
}