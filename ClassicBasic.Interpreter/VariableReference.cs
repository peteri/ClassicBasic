// <copyright file="VariableReference.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Reference to a variable, allows access to values.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VariableReference"/> class.
    /// </remarks>
    /// <param name="variable">Variable to wrap.</param>
    /// <param name="indexes">Array of indexes to access the variable, non array variables have a zero length array.</param>
    public class VariableReference(Variable variable, short[] indexes)
    {
        /// <summary>
        /// Gets a value indicating whether underlying variable is a string.
        /// </summary>
        public bool IsString => (variable.Value.GetType().GetElementType() ?? variable.Value.GetType()) == typeof(string);

        /// <summary>
        /// Gets the value of variable. If the value is a string array, converts the
        /// value to an empty string if the value is null.
        /// </summary>
        /// <returns>The value of variable.</returns>
        public Accumulator GetValue()
        {
            var type = variable.Value.GetType();
            if (typeof(Array).IsAssignableFrom(type))
            {
                object returnValue = ((Array)variable.Value).GetValue(variable.Offset(indexes));
                if ((returnValue == null) && (type.GetElementType() == typeof(string)))
                {
                    return new Accumulator(string.Empty);
                }

                return new Accumulator(returnValue);
            }

            return new Accumulator(variable.Value);
        }

        /// <summary>
        /// Sets a variable.
        /// </summary>
        /// <param name="value">Value to assign to the variable.</param>
        public void SetValue(Accumulator value)
        {
            var type = variable.Value.GetType();
            if (typeof(Array).IsAssignableFrom(type))
            {
                var array = (Array)variable.Value;
                var elementType = type.GetElementType();
                array.SetValue(value.GetValue(elementType), variable.Offset(indexes));
            }
            else
            {
                variable.Value = value.GetValue(type);
            }
        }
    }
}
