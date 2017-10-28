// <copyright file="RegisterFunctions.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Modules
{
    using Autofac;

    /// <summary>
    /// Register the commands with autofaq.
    /// </summary>
    public class RegisterFunctions : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Functions.Pos>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.LeftDollar>().As<IToken>().SingleInstance();
        }
    }
}