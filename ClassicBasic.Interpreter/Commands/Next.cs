// <copyright file="Next.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the NEXT command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Next"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="variableRepository">Variable repository.</param>
    public class Next(
        IRunEnvironment runEnvironment,
        IVariableRepository variableRepository) : Token("NEXT", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Execute the NEXT command.
        /// </summary>
        public void Execute()
        {
            StackEntry currentLoop = null;

            while (true)
            {
                currentLoop = FindForEntry(currentLoop);
                var loopVar = variableRepository.GetOrCreateVariable(currentLoop.VariableName, []);
                loopVar.SetValue(new Accumulator(loopVar.GetValue().ValueAsDouble() + currentLoop.Step));
                bool finished;
                if (currentLoop.Step > 0.0)
                {
                    finished = loopVar.GetValue().ValueAsDouble() > currentLoop.Target;
                }
                else
                {
                    finished = loopVar.GetValue().ValueAsDouble() < currentLoop.Target;
                }

                if (!finished)
                {
                    runEnvironment.CurrentLine = currentLoop.Line;
                    runEnvironment.CurrentLine.CurrentToken = currentLoop.LineToken;
                    return;
                }

                runEnvironment.ProgramStack.Pop();

                var token = runEnvironment.CurrentLine.NextToken();
                if (token.Seperator != TokenType.Comma)
                {
                    runEnvironment.CurrentLine.PushToken(token);
                    return;
                }

                token = runEnvironment.CurrentLine.NextToken();
                if (token.TokenClass == TokenClass.Variable)
                {
                    runEnvironment.CurrentLine.PushToken(token);
                }
                else
                {
                    throw new Exceptions.SyntaxErrorException();
                }
            }
        }

        private StackEntry FindForEntry(StackEntry currentLoop)
        {
            var token = runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass != TokenClass.Variable)
            {
                runEnvironment.CurrentLine.PushToken(token);
                currentLoop = runEnvironment.ProgramStack.Count > 0 ? runEnvironment.ProgramStack.Peek() : null;

                if (currentLoop?.VariableName == null)
                {
                    throw new Exceptions.NextWithoutForException();
                }
            }
            else
            {
                while (true)
                {
                    currentLoop = runEnvironment.ProgramStack.Count > 0 ? runEnvironment.ProgramStack.Peek() : null;

                    if (currentLoop?.VariableName == null)
                    {
                        throw new Exceptions.NextWithoutForException();
                    }

                    if (currentLoop.VariableName == token.Text)
                    {
                        break;
                    }

                    currentLoop = runEnvironment.ProgramStack.Pop();
                }
            }

            return currentLoop;
        }
    }
}
