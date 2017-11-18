// <copyright file="For.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the FOR command.
    /// </summary>
    public class For : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="For"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable repository.</param>
        public For(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository)
            : base("FOR", TokenClass.Statement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
        }

        /// <summary>
        /// Executes the FOR command.
        /// </summary>
        public void Execute()
        {
            var stackEntry = new StackEntry();

            // We need the name and the classic MS interpreter only supports
            // non-array double variables, so we'll do the same.
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass != TokenClass.Variable)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            stackEntry.VariableName = token.Text;
            var variableRef = _variableRepository.GetOrCreateVariable(token.Text, new short[] { });

            token = _runEnvironment.CurrentLine.NextToken();
            if (token.Seperator != TokenType.Equal)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            Accumulator startValue = _expressionEvaluator.GetExpression();
            variableRef.SetValue(startValue);

            token = _runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.To)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            stackEntry.Target = _expressionEvaluator.GetExpression().ValueAsDouble();

            token = _runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.Step)
            {
                _runEnvironment.CurrentLine.PushToken(token);
                stackEntry.Step = 1.0;
            }
            else
            {
                stackEntry.Step = _expressionEvaluator.GetExpression().ValueAsDouble();
            }

            stackEntry.Line = _runEnvironment.CurrentLine;
            stackEntry.LineToken = _runEnvironment.CurrentLine.CurrentToken;

            bool doDelete = false;
            foreach (var entry in _runEnvironment.ProgramStack)
            {
                // Gosub / Return stop searching
                if (entry.VariableName == null)
                {
                    break;
                }

                // Name matches we should overwrite.
                if (entry.VariableName == stackEntry.VariableName)
                {
                    doDelete = true;
                    break;
                }
            }

            if (doDelete)
            {
                // Pop entries off the stack until we find us.
                while (_runEnvironment.ProgramStack.Pop().VariableName != stackEntry.VariableName)
                {
                }
            }

            _runEnvironment.ProgramStack.Push(stackEntry);
            _runEnvironment.TestForStackOverflow();
        }
    }
}
