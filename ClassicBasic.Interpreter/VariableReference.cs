// <copyright file="VariableReference.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Reference to a variable, allows access to values.
    /// </summary>
    public class VariableReference
    {
        private Variable _variable;

        private short[] _indexes;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableReference"/> class.
        /// </summary>
        /// <param name="variable">Variable to wrap.</param>
        /// <param name="indexes">Array of indexes to access the variable, non array variables have a zero length array.</param>
        public VariableReference(Variable variable, short[] indexes)
        {
            _variable = variable;
            _indexes = indexes;
        }

        /// <summary>
        /// Gets the value of variable. If the value is a string array, converts the
        /// value to an empty string if the value is null.
        /// </summary>
        /// <returns>The value of variable.</returns>
        public Accumulator GetValue()
        {
            var type = _variable.Value.GetType();
            if (typeof(Array).IsAssignableFrom(type))
            {
                object returnValue = ((Array)_variable.Value).GetValue(_variable.Offset(_indexes));
                if ((returnValue == null) && (type.GetElementType() == typeof(string)))
                {
                    return new Accumulator(string.Empty);
                }

                return new Accumulator(returnValue);
            }

            return new Accumulator(_variable.Value);
        }

        /// <summary>
        /// Sets a variable.
        /// </summary>
        /// <param name="value">Value to assign to the variable.</param>
        public void SetValue(Accumulator value)
        {
            var type = _variable.Value.GetType();
            if (typeof(Array).IsAssignableFrom(type))
            {
                var array = (Array)_variable.Value;
                var elementType = type.GetElementType();
                array.SetValue(value.GetValue(elementType), _variable.Offset(_indexes));
            }
            else
            {
                _variable.Value = value.GetValue(type);
            }
        }
    }
}
