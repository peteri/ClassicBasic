// <copyright file="Load.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    /// Implements the LOAD command.
    /// </summary>
    public class Load : Token, ITokeniserCommand
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IFileSystem _fileSystem;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Load"/> class.
        /// </summary>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="fileSystem">Mockable file system.</param>
        /// <param name="programRepository">Program repository.</param>
        public Load(
            IExpressionEvaluator expressionEvaluator,
            IFileSystem fileSystem,
            IProgramRepository programRepository)
            : base("LOAD", TokenClass.Statement)
        {
            _expressionEvaluator = expressionEvaluator;
            _fileSystem = fileSystem;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the LOAD comand.
        /// </summary>
        /// <param name="tokeniser">Tokeniser to use.</param>
        public void Execute(ITokeniser tokeniser)
        {
            var fileName = _expressionEvaluator.GetExpression().ValueAsString();
            int lastProgramLine = 0;
            try
            {
                var program = _fileSystem.File.ReadAllLines(fileName, System.Text.Encoding.UTF8);
                _programRepository.Clear();
                foreach (var line in program)
                {
                    var programLine = tokeniser.Tokenise(line);
                    if (!programLine.LineNumber.HasValue)
                    {
                        throw new Exception("MISSING LINE NUMBER");
                    }

                    lastProgramLine = programLine.LineNumber.Value;
                    _programRepository.SetProgramLine(programLine);
                }
            }
            catch (Exception ex)
            {
                throw new Exceptions.BasicException($"BAD LOAD {ex.Message}, LAST GOOD LINE WAS {lastProgramLine}", 100);
            }
        }
    }
}
