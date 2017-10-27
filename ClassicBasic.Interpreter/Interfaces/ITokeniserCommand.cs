// <copyright file="ITokeniserCommand.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Interface that is used to pass in a tokeniser to IToken commands that need it.
    /// Required as adding ITokeniser to command ends up causing a circular reference
    /// in the constructors.
    /// </summary>
    public interface ITokeniserCommand
    {
        /// <summary>
        /// Executes a command that requires a tokeniser.
        /// </summary>
        /// <param name="tokeniser">ITokeniser to use.</param>
        void Execute(ITokeniser tokeniser);
    }
}
