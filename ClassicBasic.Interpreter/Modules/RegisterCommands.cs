// <copyright file="RegisterCommands.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Modules
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Register the commands with Microsoft Dependency Injection.
    /// </summary>
    public class RegisterCommands
    {
        /// <summary>
        /// Registers commands with dependency injection.
        /// </summary>
        /// <param name="services">IServiceCollection to register types in.</param>
        public static void Load(IServiceCollection services)
        {
            services.AddSingleton<IToken, Commands.Clear>();
            services.AddSingleton<IToken, Commands.Clear>();
            services.AddSingleton<IToken, Commands.Cont>();
            services.AddSingleton<IToken, Commands.Data>();
            services.AddSingleton<IToken, Commands.Def>();
            services.AddSingleton<IToken, Commands.Del>();
            services.AddSingleton<IToken, Commands.Dim>();
            services.AddSingleton<IToken, Commands.Edit>();
            services.AddSingleton<IToken, Commands.End>();
            services.AddSingleton<IToken, Commands.Else>();
            services.AddSingleton<IToken, Commands.For>();
            services.AddSingleton<IToken, Commands.Get>();
            services.AddSingleton<IToken, Commands.Gosub>();
            services.AddSingleton<IToken, Commands.Goto>();
            services.AddSingleton<IToken, Commands.If>();
            services.AddSingleton<IToken, Commands.Input>();
            services.AddSingleton<IToken, Commands.Let>();
            services.AddSingleton<IToken, Commands.List>();
            services.AddSingleton<IToken, Commands.Load>();
            services.AddSingleton<IToken, Commands.New>();
            services.AddSingleton<IToken, Commands.Next>();
            services.AddSingleton<IToken, Commands.On>();
            services.AddSingleton<IToken, Commands.OnErr>();
            services.AddSingleton<IToken, Commands.Pop>();
            services.AddSingleton<IToken, Commands.Print>();
            services.AddSingleton<IToken, Commands.Read>();
            services.AddSingleton<IToken, Commands.Remark>();
            services.AddSingleton<IToken, Commands.Restore>();
            services.AddSingleton<IToken, Commands.Resume>();
            services.AddSingleton<IToken, Commands.Return>();
            services.AddSingleton<IToken, Commands.Run>();
            services.AddSingleton<IToken, Commands.Save>();
            services.AddSingleton<IToken, Commands.Stop>();
        }
    }
}
