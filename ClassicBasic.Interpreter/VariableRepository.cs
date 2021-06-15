// <copyright file="VariableRepository.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Repository of variables.
    /// </summary>
    public class VariableRepository : IVariableRepository
    {
        private readonly Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();

        /// <summary>
        /// Clears the repository.
        /// </summary>
        public void Clear()
        {
            _variables.Clear();
        }

        /// <summary>
        /// Creates an array variable.
        /// </summary>
        /// <param name="name">Name of the variable (without array brackets).</param>
        /// <param name="dimensions">Dimensions to create array with.</param>
        public void DimensionArray(string name, short[] dimensions)
        {
            var nameKey = ShortenName(name) + "(";

            if (_variables.ContainsKey(nameKey))
            {
                throw new Exceptions.RedimensionedArrayException();
            }

            var variable = new Variable(name, dimensions);
            _variables.Add(variable.Name, variable);
        }

        /// <summary>
        /// Gets a reference to a variable using the indexes if the variable is an array.
        /// If the varible is unknown, the variable is created, arrays are created
        /// with dimensions of ten. Arrays variables have an index range starting of
        /// 0 .. size inclusive i.e. an default array has 11 elements.
        /// </summary>
        /// <param name="name">Name of the variable (without array brackets).</param>
        /// <param name="indexes">Indexes to use if the variable is an array.</param>
        /// <returns>Reference to variable.</returns>
        public VariableReference GetOrCreateVariable(string name, short[] indexes)
        {
            var nameKey = ShortenName(name);

            if (!_variables.TryGetValue(nameKey + ((indexes.Length == 0) ? string.Empty : "("), out Variable variable))
            {
                if (indexes.Length == 0)
                {
                    variable = new Variable(nameKey, indexes);
                }
                else
                {
                    var dimensions = Enumerable.Repeat<short>(10, indexes.Length).ToArray();
                    variable = new Variable(nameKey, dimensions);
                }

                _variables.Add(variable.Name, variable);
            }

            return new VariableReference(variable, indexes);
        }

        /// <summary>
        /// Shortens a name to two characters plus optional type character.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <returns>Shorten version of the variable.</returns>
        private string ShortenName(string name)
        {
            string nameKey = name;

            if (name.Length > 2)
            {
                nameKey = name.Substring(0, 2);
                var lastChar = name[name.Length - 1];
                if ((lastChar == '$') || (lastChar == '%'))
                {
                    nameKey += lastChar;
                }
            }

            return nameKey;
        }
    }
}
