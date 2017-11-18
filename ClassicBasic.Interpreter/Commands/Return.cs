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

        /// <summary>
        /// Initializes a new instance of the <see cref="Return"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Return(IRunEnvironment runEnvironment)
            : base("RETURN", TokenClass.Statement)
        {
            _runEnvironment = runEnvironment;
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
            while (stackEntry.VariableName != null);

            _runEnvironment.CurrentLine = stackEntry.Line;
            _runEnvironment.CurrentLine.CurrentToken = stackEntry.LineToken;
        }
    }
}
