// <copyright file="Interpreter.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// The main interpreter.
    /// </summary>
    public class Interpreter : IInterpreter
    {
        private readonly ITeletypeWithPosition _teletypeWithPosition;
        private readonly ITokeniser _tokeniser;
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;
        private readonly IExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpreter"/> class.
        /// </summary>
        /// <param name="teletypeWithPosition">Teletype to use for input output</param>
        /// <param name="tokeniser">Tokeniser.</param>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program repository.</param>
        /// <param name="executor">Executor class to run program lines.</param>
        public Interpreter(
            ITeletypeWithPosition teletypeWithPosition,
            ITokeniser tokeniser,
            IRunEnvironment runEnvironment,
            IProgramRepository programRepository,
            IExecutor executor)
        {
            _teletypeWithPosition = teletypeWithPosition;
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
                _teletypeWithPosition.NewLine();
                _teletypeWithPosition.Write(">");
                var command = _teletypeWithPosition.Read();
                if (command == null)
                {
                    _runEnvironment.KeyboardBreak = false;
                    continue;
                }

                try
                {
                    var parsedLine = _tokeniser.Tokenise(command);
                    if (parsedLine.LineNumber.HasValue)
                    {
                        _programRepository.SetProgramLine(parsedLine);
                    }
                    else
                    {
                        _runEnvironment.CurrentLine = parsedLine;
                        quit = _executor.ExecuteLine();
                    }
                }
                catch (Exceptions.BreakException endError)
                {
                    if (endError.ErrorMessage != string.Empty)
                    {
                        WriteErrorToTeletype(
                            _runEnvironment.DataErrorLine ?? _runEnvironment.CurrentLine.LineNumber,
                            endError.ErrorMessage);
                    }
                }
                catch (Exceptions.BasicException basicError)
                {
                    WriteErrorToTeletype(
                            _runEnvironment.DataErrorLine ?? _runEnvironment.CurrentLine?.LineNumber,
                        "?" + basicError.ErrorMessage + " ERROR");
                }
            }
        }

        private void WriteErrorToTeletype(int? lineNumber, string message)
        {
            _teletypeWithPosition.NewLine();
            _teletypeWithPosition.Write(message + (lineNumber.HasValue ? $" IN {lineNumber}" : string.Empty));
            _teletypeWithPosition.NewLine();
        }
    }
}
