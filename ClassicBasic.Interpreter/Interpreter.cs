// <copyright file="Interpreter.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// The main interpreter.
    /// </summary>
    public class Interpreter : IInterpreter
    {
        private readonly ITeletype _teletype;
        private readonly ITokeniser _tokeniser;
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpreter"/> class.
        /// </summary>
        /// <param name="teletype">Teletype to use for input output</param>
        /// <param name="tokeniser">Tokeniser.</param>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program repository.</param>
        /// <param name="executor">Executor class to run program lines.</param>
        public Interpreter(
            ITeletype teletype,
            ITokeniser tokeniser,
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExecutor executor)
        {
            _teletype = teletype;
            _tokeniser = tokeniser;
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
            _executor = executor;
        }

        /// <summary>
        /// Executes the interpreter.
        /// </summary>
        public void Execute()
        {
            bool quit = false;
            while (!quit)
            {
                _teletype.Write(Environment.NewLine);
                _teletype.Write(">");
                var command = _teletype.Read();
                if (command == null)
                {
                    _runEnvironment.KeyboardBreak = false;
                    continue;
                }

                var parsedLine = _tokeniser.Tokenise(command);
                if (parsedLine.LineNumber.HasValue)
                {
                    if (parsedLine.EndOfLine)
                    {
                        _programRepository.DeleteProgramLines(
                            parsedLine.LineNumber.Value,
                            parsedLine.LineNumber.Value);
                    }
                    else
                    {
                        _programRepository.SetProgramLine(parsedLine);
                    }
                }
                else
                {
                    _runEnvironment.CurrentLine = parsedLine;
                    try
                    {
                        quit = _executor.ExecuteLine();
                    }
                    catch (Exceptions.BreakException endError)
                    {
                        if (endError.ErrorMessage != string.Empty)
                        {
                            WriteErrorToTeletype(_runEnvironment.ContinueLineNumber, endError.ErrorMessage);
                        }
                    }
                    catch (Exceptions.BasicException basicError)
                    {
                        WriteErrorToTeletype(
                            _runEnvironment.CurrentLine.LineNumber,
                            "?" + basicError.ErrorMessage + " ERROR");
                    }
                }
            }
        }

        private void WriteErrorToTeletype(int? lineNumber, string message)
        {
            _teletype.Write(
                Environment.NewLine +
                message +
                (lineNumber.HasValue ? $" IN {lineNumber}" : string.Empty)
                + Environment.NewLine);
        }
    }
}
