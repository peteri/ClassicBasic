// <copyright file="Clear.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the clear command.
    /// </summary>
    public class Clear : Token, ICommand
    {
        private readonly IVariableRepository _variableRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Clear"/> class.
        /// </summary>
        /// <param name="variableRepository">Variable repository to clear.</param>
        public Clear(IVariableRepository variableRepository)
            : base("CLEAR", TokenType.ClassStatement)
        {
            _variableRepository = variableRepository;
        }

        /// <summary>
        /// Executes the CLEAR command.
        /// </summary>
        public void Execute()
        {
            _variableRepository.Clear();
        }
    }
}
