﻿// <copyright file="Enums.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;

    /// <summary>
    /// Token enum.
    /// </summary>
    [Flags]
    public enum TokenType
    {
        /// <summary>Unknown</summary>
        Unknown = 0,

        /// <summary>Token is a colon.</summary>
        Colon,

        /// <summary>Token is a open bracket.</summary>
        OpenBracket,

        /// <summary>Token is a close bracket.</summary>
        CloseBracket,

        /// <summary>Token is a comma.</summary>
        Comma,

        /// <summary>Token is a semi colon.</summary>
        Semicolon,

        /// <summary>Token is a dollar.</summary>
        Dollar,

        /// <summary>Token is a percent.</summary>
        Percent,

        /// <summary>Token is a end of line.</summary>
        EndOfLine,

        /// <summary>Token is a plus.</summary>
        Plus,

        /// <summary>Token is a minus.</summary>
        Minus,

        /// <summary>Token is a multiply.</summary>
        Multiply,

        /// <summary>Token is a divide.</summary>
        Divide,

        /// <summary>Token is a and.</summary>
        And,

        /// <summary>Token is a or.</summary>
        Or,

        /// <summary>Token is a greater than.</summary>
        GreaterThan,

        /// <summary>Token is a less than.</summary>
        LessThan,

        /// <summary>Token is an equal.</summary>
        Equal,

        /// <summary>Token is not.</summary>
        Not,

        /// <summary>Token is a power seperator.</summary>
        Power,

        /// <summary>Token is for a TAB( statement.</summary>
        Tab,

        /// <summary>Token is for a SPC( statement.</summary>
        Space,

        /// <summary>Token is for a TO statement.</summary>
        To,

        /// <summary>Token is for a DATA statement.</summary>
        Data,

        /// <summary>Token is for a STEP statement.</summary>
        Step,

        /// <summary>Token is for a THEN statement.</summary>
        Then,

        /// <summary>Token is for a ELSE statement.</summary>
        Else,

        /// <summary>Token is for a SYSTEM statement.</summary>
        System,

        /// <summary>Token is for a LET statement.</summary>
        Let,

        /// <summary>Token is for a Remark statement</summary>
        Remark,

        /// <summary>Token is for a Goto statement</summary>
        Goto,

        /// <summary>Token is for a Gosub statement</summary>
        Gosub,

        /// <summary>Token is for a FN statement</summary>
        Fn,

        /// <summary>Token is for a PRINT statement</summary>
        Print,
    }

    /// <summary>
    /// Class of the token.
    /// </summary>
    public enum TokenClass
    {
        /// <summary>Unknown</summary>
        Unknown = 0,

        /// <summary>Token is textual bit of a data statement.</summary>
        Data,

        /// <summary>Token is a statement.</summary>
        Statement,

        /// <summary>Token is a function.</summary>
        Function,

        /// <summary>Token is a "string" user typed in program.</summary>
        String,

        /// <summary>Token could be a variable.</summary>
        Variable,

        /// <summary>Token could be a number.</summary>
        Number,

        /// <summary>Token is a seperator.</summary>
        Seperator,

        /// <summary>Token is textual bit of a remark statement.</summary>
        Remark,
    }
}
