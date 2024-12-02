// <copyright file="Pop.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the POP command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Pop"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class Pop(IRunEnvironment runEnvironment) : Token("POP", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the POP command.
        /// </summary>
        public void Execute()
        {
            StackEntry stackEntry;
            do
            {
                if (runEnvironment.ProgramStack.Count == 0)
                {
                    throw new Exceptions.ReturnWithoutGosubException();
                }

                stackEntry = runEnvironment.ProgramStack.Pop();
            }
            while (stackEntry.VariableName != null);
        }
    }
}
