// <copyright file="IInterruptableCommand.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Interface for commands which have a setup phase (to parse command
    /// parameters) and a longer execute phase.
    /// </summary>
    public interface IInterruptableCommand
    {
        /// <summary>
        /// Setup for the command.
        /// </summary>
        void Setup();

        /// <summary>
        /// Execute is repeatably called until the command returns true.
        /// </summary>
        /// <returns>true when the command should stop.</returns>
        bool Execute();
    }
}
