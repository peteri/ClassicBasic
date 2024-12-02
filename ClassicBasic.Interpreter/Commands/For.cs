// <copyright file="For.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the FOR command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="For"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="expressionEvaluator">Expression evaluator.</param>
    /// <param name="variableRepository">Variable repository.</param>
    public class For(
        IRunEnvironment runEnvironment,
        IExpressionEvaluator expressionEvaluator,
        IVariableRepository variableRepository) : Token("FOR", TokenClass.Statement), ICommand
    {
        /// <summary>
        /// Executes the FOR command.
        /// </summary>
        public void Execute()
        {
            var stackEntry = new StackEntry();

            // We need the name and the classic MS interpreter only supports
            // non-array double variables, so we'll do the same.
            var token = runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass != TokenClass.Variable)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            stackEntry.VariableName = token.Text;
            var variableRef = variableRepository.GetOrCreateVariable(token.Text, []);

            token = runEnvironment.CurrentLine.NextToken();
            if (token.Seperator != TokenType.Equal)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            Accumulator startValue = expressionEvaluator.GetExpression();
            variableRef.SetValue(startValue);

            token = runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.To)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            stackEntry.Target = expressionEvaluator.GetExpression().ValueAsDouble();

            token = runEnvironment.CurrentLine.NextToken();
            if (token.Statement != TokenType.Step)
            {
                runEnvironment.CurrentLine.PushToken(token);
                stackEntry.Step = 1.0;
            }
            else
            {
                stackEntry.Step = expressionEvaluator.GetExpression().ValueAsDouble();
            }

            stackEntry.Line = runEnvironment.CurrentLine;
            stackEntry.LineToken = runEnvironment.CurrentLine.CurrentToken;

            bool doDelete = false;
            foreach (var entry in runEnvironment.ProgramStack)
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
                while (runEnvironment.ProgramStack.Pop().VariableName != stackEntry.VariableName)
                {
                }
            }

            runEnvironment.ProgramStack.Push(stackEntry);
            runEnvironment.TestForStackOverflow();
        }
    }
}
