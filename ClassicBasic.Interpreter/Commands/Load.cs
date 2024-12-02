// <copyright file="Load.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    /// Implements the LOAD command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Load"/> class.
    /// </remarks>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="fileSystem">Mockable file system.</param>
    /// <param name="programRepository">Program repository.</param>
    public class Load(
        IExpressionEvaluator expressionEvaluator,
        IFileSystem fileSystem,
        IProgramRepository programRepository) : Token("LOAD", TokenClass.Statement), ITokeniserCommand
    {
        /// <summary>
        /// Executes the LOAD comand.
        /// </summary>
        /// <param name="tokeniser">Tokeniser to use.</param>
        public void Execute(ITokeniser tokeniser)
        {
            var fileName = expressionEvaluator.GetExpression().ValueAsString();
            int lastProgramLine = 0;
            try
            {
                var program = fileSystem.File.ReadAllLines(fileName, System.Text.Encoding.UTF8);
                programRepository.Clear();
                foreach (var line in program)
                {
                    var programLine = tokeniser.Tokenise(line);
                    if (!programLine.LineNumber.HasValue)
                    {
                        throw new Exception("MISSING LINE NUMBER");
                    }

                    lastProgramLine = programLine.LineNumber.Value;
                    programRepository.SetProgramLine(programLine);
                }
            }
            catch (Exception ex)
            {
                throw new Exceptions.BasicException($"BAD LOAD {ex.Message}, LAST GOOD LINE WAS {lastProgramLine}", 100);
            }
        }
    }
}
