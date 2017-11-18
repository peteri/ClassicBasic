// <copyright file="Pop.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the POP command.
    /// </summary>
    public class Pop : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        public Pop(IRunEnvironment runEnvironment)
            : base("POP", TokenClass.Statement)
        {
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Executes the POP command.
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
        }
    }
}
