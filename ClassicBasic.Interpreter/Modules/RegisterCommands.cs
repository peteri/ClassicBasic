// <copyright file="RegisterCommands.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Modules
{
    using Autofac;

    /// <summary>
    /// Register the commands with autofaq.
    /// </summary>
    public class RegisterCommands : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Commands.Cont>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Del>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Dim>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.End>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Else>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.For>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Gosub>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Goto>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.If>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Input>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Let>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.List>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Load>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Next>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.OnErr>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Pop>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Print>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Read>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Remark>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Restore>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Resume>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Return>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Run>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Save>().As<IToken>().SingleInstance();
            builder.RegisterType<Commands.Stop>().As<IToken>().SingleInstance();
        }
    }
}
