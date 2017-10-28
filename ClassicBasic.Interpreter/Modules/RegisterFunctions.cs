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
            builder.RegisterType<Functions.Abs>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Asc>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Atn>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.CharDollar>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Cos>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Exp>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Fre>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Int>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.LeftDollar>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Len>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Log>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.MidDollar>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Pos>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.RightDollar>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Rnd>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Sgn>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Sin>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Sqr>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.StrDollar>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Tan>().As<IToken>().SingleInstance();
            builder.RegisterType<Functions.Val>().As<IToken>().SingleInstance();
        }
    }
}