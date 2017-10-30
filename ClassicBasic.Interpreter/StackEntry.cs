// <copyright file="StackEntry.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    /// <summary>
    /// Entry on the program stack used for FOR/NEXT GOSUB/RETURN
    /// If the variable is null then it's an gosub/return entry.
    /// </summary>
    public class StackEntry
    {
        /// <summary>
        /// Gets or sets line number to loop back to (or return too)
        /// </summary>
        public int? LineNumber { get; set; }

        /// <summary>
        /// Gets or sets line token to loop back to (or return too)
        /// </summary>
        public int LineToken { get; set; }

        /// <summary>
        /// Gets or sets the variable name
        /// </summary>
        public string VariableRef { get; set; }

        /// <summary>
        /// Gets or sets target value for next loop.
        /// </summary>
        public double Target { get; set; }

        /// <summary>
        /// Gets or sets step value.
        /// </summary>
        public double Step { get; set; }
    }
}
