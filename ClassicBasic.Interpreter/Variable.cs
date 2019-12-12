// <copyright file="Variable.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Class representing a variable.
    /// </summary>
    public class Variable
    {
        private readonly short[] _dimensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// Infers the type from the last letter of the name.
        /// </summary>
        /// <param name="name">Name of variable without any brackets.</param>
        /// <param name="dimensions">Array of shorts representing dimensions of array,
        /// if empty array passed raw variable is created.
        /// </param>
        public Variable(string name, short[] dimensions)
        {
            int size = 1;
            for (int i = 0; i < dimensions.Length; i++)
            {
                size *= dimensions[i] + 1;
                if ((size <= 0) || (size > 0x10000))
                {
                    throw new Exceptions.OutOfMemoryException();
                }
            }

            Name = name + ((size != 1) ? "(" : string.Empty);

            _dimensions = dimensions;

            if (name.EndsWith("$"))
            {
                Value = (size == 1) ? string.Empty : (object)new string[size];
            }
            else if (name.EndsWith("%"))
            {
                Value = (size == 1) ? (short)0 : (object)new short[size];
            }
            else
            {
                Value = (size == 1) ? 0.0 : (object)new double[size];
            }
        }

        /// <summary>
        /// Gets or set name of the variable, if the variable is an array an opening bracket is added.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets value of the variable.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Calculates the offset of an array element.
        /// </summary>
        /// <param name="indexes">Indexes to calculate.</param>
        /// <returns>Index into the array.</returns>
        public int Offset(short[] indexes)
        {
            if (indexes.Length != _dimensions.Length)
            {
                throw new Exceptions.BadSubscriptException();
            }

            int offset = 0;
            int multiplier = 1;
            for (int i = 0; i < _dimensions.Length; i++)
            {
                if ((indexes[i] < 0) || (indexes[i] > _dimensions[i]))
                {
                    throw new Exceptions.BadSubscriptException();
                }

                offset += indexes[i] * multiplier;
                multiplier *= _dimensions[i] + 1;
            }

            return offset;
        }
    }
}