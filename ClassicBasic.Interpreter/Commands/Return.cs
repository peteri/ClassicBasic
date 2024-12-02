// <copyright file="Return.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RETURN command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Return"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    public class Return(IRunEnvironment runEnvironment) : Token("RETURN", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the return command.
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

            runEnvironment.CurrentLine = stackEntry.Line;
            runEnvironment.CurrentLine.CurrentToken = stackEntry.LineToken;
        }
    }
}
