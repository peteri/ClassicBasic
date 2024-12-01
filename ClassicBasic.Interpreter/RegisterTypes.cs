// <copyright file="RegisterTypes.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.IO.Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Register the types for the interpreter.
    /// </summary>
    public class RegisterTypes
    {
        /// <summary>
        /// Registers the types contained in the interpreter.
        /// </summary>
        /// <param name="services">IServiceCollection to register types in.</param>
        public static void Register(IServiceCollection services)
        {
            Modules.RegisterCommands.Load(services);
            Modules.RegisterFunctions.Load(services);

            // Other stuff we care about
            services.AddSingleton<IExecutor, Executor>();
            services.AddSingleton<ITokeniser, Tokeniser>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IInterpreter, Interpreter>();
            services.AddSingleton<IRunEnvironment, RunEnvironment>();
            services.AddSingleton<ITokensProvider, TokensProvider>();
            services.AddSingleton<IProgramRepository, ProgramRepository>();
            services.AddSingleton<IVariableRepository, VariableRepository>();
            services.AddSingleton<IDataStatementReader, DataStatementReader>();
            services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();
            services.AddSingleton<ITeletypeWithPosition, TeletypeWithPosition>();
        }
    }
}
