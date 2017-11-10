// <copyright file="Accumulator.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Accumulator may contain either strings or numbers.
    /// </summary>
    public class Accumulator
    {
        private object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Accumulator"/> class.
        /// </summary>
        /// <param name="initialValue">Initial value of the accumulator.</param>
        public Accumulator(object initialValue)
        {
            _value = initialValue;
            Type = initialValue.GetType();
            TestOverlongString();
        }

        /// <summary>
        /// Gets the dot net type of the value.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the value as a double. Auto converts between
        /// shorts and doubles. Throws a type mismatch otherwise.
        /// </summary>
        /// <returns>Value as a double.</returns>
        public double ValueAsDouble()
        {
            if (Type == typeof(double))
            {
                return (double)_value;
            }

            if (Type == typeof(short))
            {
                return (short)_value;
            }

            throw new Exceptions.TypeMismatchException();
        }

        /// <summary>
        /// Gets the value as a string. Throws if value is not a string.
        /// </summary>
        /// <returns>Value as a string.</returns>
        public string ValueAsString()
        {
            if (Type == typeof(string))
            {
                return (string)_value;
            }

            throw new Exceptions.TypeMismatchException();
        }

        /// <summary>
        /// Gets the value as a shory. Auto converts doubles, throws if the
        /// value is not convertible or is too big for a short.
        /// </summary>
        /// <returns>Value as short.</returns>
        public short ValueAsShort()
        {
            if (Type == typeof(double))
            {
                double value = Math.Floor((double)_value);
                if ((value < short.MinValue) || (value > short.MaxValue))
                {
                    throw new Exceptions.IllegalQuantityException();
                }

                return (short)value;
            }

            if (Type == typeof(short))
            {
                return (short)_value;
            }

            throw new Exceptions.TypeMismatchException();
        }

        /// <summary>
        /// Gets the current value of the accumulator converted to the expected type.
        /// Throws if types do not match.
        /// </summary>
        /// <param name="expectedType">Expected type.</param>
        /// <returns>Value as an object.</returns>
        public object GetValue(Type expectedType)
        {
            if (expectedType == typeof(short))
            {
                return ValueAsShort();
            }

            if (expectedType == typeof(double))
            {
                return ValueAsDouble();
            }

            if (Type != expectedType)
            {
                throw new Exceptions.TypeMismatchException();
            }

            return _value;
        }

        /// <summary>
        /// Sets the value to a new value.
        /// </summary>
        /// <param name="newValue">Value to set the accumulator to.</param>
        public void SetValue(object newValue)
        {
            _value = newValue;
            Type = newValue.GetType();
            TestOverlongString();
        }

        /// <summary>
        /// Override so we can print stuff out for listings.
        /// </summary>
        /// <returns>Converted version of string.</returns>
        public override string ToString()
        {
            var type = _value.GetType().GetElementType() ?? _value.GetType();
            if (type == typeof(string))
            {
                return _value.ToString();
            }
            else
            {
                return " " + _value.ToString() + " ";
            }
        }

        private void TestOverlongString()
        {
            if (_value is string text && (text.Length > 256))
            {
                throw new Exceptions.StringToLongException();
            }
        }
    }
}