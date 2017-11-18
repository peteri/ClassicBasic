// <copyright file="Save.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Text;

    /// <summary>
    /// Implements the Save command.
    /// </summary>
    public class Save : Token, ICommand
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramRepository _programRepository;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="Save"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="programRepository">Program Repository.</param>
        /// <param name="fileSystem">Filesystem to use.</param>
        public Save(
            IExpressionEvaluator expressionEvaluator,
            IProgramRepository programRepository,
            IFileSystem fileSystem)
            : base("SAVE", TokenClass.Statement)
        {
            _expressionEvaluator = expressionEvaluator;
            _programRepository = programRepository;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Implements the SAVE command.
        /// </summary>
        public void Execute()
        {
            var filename = _expressionEvaluator.GetExpression().ValueAsString();

            var lines = new List<string>();
            var currentLine = _programRepository.GetFirstLine();
            while (currentLine != null)
            {
                lines.Add(currentLine.ToString());
                currentLine = _programRepository.GetNextLine(currentLine.LineNumber.Value);
            }

            try
            {
                _fileSystem.File.WriteAllLines(filename, lines, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exceptions.BasicException($"BAD SAVE {ex.Message}.", 101);
            }
        }
    }
}
