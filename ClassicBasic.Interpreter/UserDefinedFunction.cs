// <copyright file="UserDefinedFunction.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Function definition.
    /// </summary>
    public class UserDefinedFunction
    {
        /// <summary>
        /// Gets or sets line contents the function executes.
        /// </summary>
        public ProgramLine Line { get; set; }

        /// <summary>
        /// Gets or sets line token the function starts on.
        /// </summary>
        public int LineToken { get; set; }

        /// <summary>
        /// Gets or sets the function name.
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets or sets the variable name
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Executes a user defined function.
        /// </summary>
        /// <param name="parameter">new parameter value.</param>
        /// <param name="runEnvironment">Run evironment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable repository.</param>
        /// <returns>Value of function.</returns>
        public Accumulator Execute(
            Accumulator parameter,
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository)
        {
            // Save the variable and program line.
            var savedVariable = variableRepository.GetOrCreateVariable(VariableName, new short[] { }).GetValue();
            var savedProgramLine = runEnvironment.CurrentLine;

            // Change the variable to be our parameter.
            variableRepository.GetOrCreateVariable(VariableName, new short[] { }).SetValue(parameter);
            runEnvironment.CurrentLine = Line;
            runEnvironment.CurrentLine.CurrentToken = LineToken;

            // Evaluate the expression.
            var returnValue = expressionEvaluator.GetExpression();

            // Restore the variable and program line.
            variableRepository.GetOrCreateVariable(VariableName, new short[] { }).SetValue(savedVariable);
            runEnvironment.CurrentLine = savedProgramLine;

            // return the value.
            return returnValue;
        }
    }
}
