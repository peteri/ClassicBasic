// <copyright file="RegisterFunctions.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Modules
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Register the functions with Microsoft Dependency Injection.
    /// </summary>
    public class RegisterFunctions
    {
        public static void Load(IServiceCollection services)
        {
            services.AddSingleton<IToken, Functions.Abs>();
            services.AddSingleton<IToken, Functions.Asc>();
            services.AddSingleton<IToken, Functions.Atn>();
            services.AddSingleton<IToken, Functions.CharDollar>();
            services.AddSingleton<IToken, Functions.Cos>();
            services.AddSingleton<IToken, Functions.Exp>();
            services.AddSingleton<IToken, Functions.Fre>();
            services.AddSingleton<IToken, Functions.Int>();
            services.AddSingleton<IToken, Functions.LeftDollar>();
            services.AddSingleton<IToken, Functions.Len>();
            services.AddSingleton<IToken, Functions.Log>();
            services.AddSingleton<IToken, Functions.MidDollar>();
            services.AddSingleton<IToken, Functions.Pos>();
            services.AddSingleton<IToken, Functions.RightDollar>();
            services.AddSingleton<IToken, Functions.Rnd>();
            services.AddSingleton<IToken, Functions.Sgn>();
            services.AddSingleton<IToken, Functions.Sin>();
            services.AddSingleton<IToken, Functions.Sqr>();
            services.AddSingleton<IToken, Functions.StrDollar>();
            services.AddSingleton<IToken, Functions.Tan>();
            services.AddSingleton<IToken, Functions.Val>();
        }
    }
}