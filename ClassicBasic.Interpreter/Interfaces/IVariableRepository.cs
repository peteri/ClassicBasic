// <copyright file="IVariableRepository.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Variable repository, retrieves and creates variables reference.
    /// </summary>
    public interface IVariableRepository
    {
        /// <summary>
        /// Clears the repository.
        /// </summary>
        void Clear();

        /// <summary>
        /// Creates an array variable.
        /// </summary>
        /// <param name="name">Name of the variable (without array brackets).</param>
        /// <param name="dimensions">Dimensions to create array with.</param>
        void DimensionArray(string name, short[] dimensions);

        /// <summary>
        /// Gets a reference to a variable using the indexes if the variable is an array.
        /// If the varible is unknown, the variable is created, arrays are created
        /// with dimensions of ten. Arrays variables have an index range starting of
        /// 0 .. size inclusive i.e. an default array has 11 elements.
        /// </summary>
        /// <param name="name">Name of the variable (without array brackets).</param>
        /// <param name="indexes">Indexes to use if the variable is an array.</param>
        /// <returns>Reference to variable.</returns>
        VariableReference GetOrCreateVariable(string name, short[] indexes);
    }
}