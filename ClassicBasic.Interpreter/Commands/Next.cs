// <copyright file="Next.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the NEXT command.
    /// </summary>
    public class Next : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IVariableRepository _variableRepository;
        private readonly IProgramRepository _programRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Next"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="variableRepository">Variable repository.</param>
        /// <param name="programRepository">Program repository.</param>
        public Next(
            IRunEnvironment runEnvironment,
            IVariableRepository variableRepository,
            IProgramRepository programRepository)
            : base("NEXT", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _variableRepository = variableRepository;
            _programRepository = programRepository;
        }

        /// <summary>
        /// Execute the NEXT command.
        /// </summary>
        public void Execute()
        {
            StackEntry currentLoop = null;

            while (true)
            {
                currentLoop = FindForEntry(currentLoop);
                var loopVar = _variableRepository.GetOrCreateVariable(currentLoop.VariableRef, new short[] { });
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
                    if (currentLoop.LineNumber.HasValue)
                    {
                        _runEnvironment.CurrentLine = _programRepository.GetLine(currentLoop.LineNumber.Value);
                    }

                    _runEnvironment.CurrentLine.CurrentToken = currentLoop.LineToken;
                    return;
                }

                _runEnvironment.ProgramStack.Pop();

                var token = _runEnvironment.CurrentLine.NextToken();
                if (token.Seperator != TokenType.Comma)
                {
                    _runEnvironment.CurrentLine.PushToken(token);
                    return;
                }

                token = _runEnvironment.CurrentLine.NextToken();
                if (token.TokenClass == TokenType.ClassVariable)
                {
                    _runEnvironment.CurrentLine.PushToken(token);
                }
                else
                {
                    throw new Exceptions.SyntaxErrorException();
                }
            }
        }

        private StackEntry FindForEntry(StackEntry currentLoop)
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass != TokenType.ClassVariable)
            {
                _runEnvironment.CurrentLine.PushToken(token);
                if (_runEnvironment.ProgramStack.Count > 0)
                {
                    currentLoop = _runEnvironment.ProgramStack.Peek();
                }

                if (currentLoop?.VariableRef == null)
                {
                    throw new Exceptions.NextWithoutForException();
                }
            }
            else
            {
                while (true)
                {
                    if (_runEnvironment.ProgramStack.Count > 0)
                    {
                        currentLoop = _runEnvironment.ProgramStack.Peek();
                    }

                    if (currentLoop?.VariableRef == null)
                    {
                        throw new Exceptions.NextWithoutForException();
                    }

                    if (currentLoop.VariableRef == token.Text)
                    {
                        break;
                    }

                    currentLoop = _runEnvironment.ProgramStack.Pop();
                }
            }

            return currentLoop;
        }
    }
}
