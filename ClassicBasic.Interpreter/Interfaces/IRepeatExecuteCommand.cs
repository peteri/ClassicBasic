// <copyright file="IRepeatExecuteCommand.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// This means re-run the execute command logic once this command has finished.
    /// Used by the IF THEN ELSE logic, stops the check for a : biting us.
    /// </summary>
    public interface IRepeatExecuteCommand
    {
    }
}
