// <copyright file="Return.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RETURN command.
    /// </summary>
    public class Return : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Return"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="programRepository">Program repository.</param>
        public Return(IRunEnvironment runEnvironment, IProgramRepository programRepository)
            : base("RETURN", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Executes the return command.
        /// </summary>
        public void Execute()
        {
            StackEntry stackEntry;
            do
            {
                if (_runEnvironment.ProgramStack.Count == 0)
                {
                    throw new Exceptions.ReturnWithoutGosubException();
                }

                stackEntry = _runEnvironment.ProgramStack.Pop();
            }
            while (stackEntry.VariableRef != null);

            if (stackEntry.LineNumber.HasValue)
            {
                _runEnvironment.CurrentLine = _programRepository.GetLine(stackEntry.LineNumber.Value);
            }

            _runEnvironment.CurrentLine.CurrentToken = stackEntry.LineToken;
        }
    }
}
